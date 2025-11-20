using Microsoft.EntityFrameworkCore;
using SuporteTI.Api.Data;
using SuporteTI.Shared.Models;
using System.Text.RegularExpressions;
using Azure.AI.OpenAI; // <-- Usado pelo OpenAI
using Azure;             // <-- Usado pelo OpenAI
using Microsoft.Extensions.Configuration; // <-- Usado para pegar o nome do modelo

namespace SuporteTI.Api.Services
{
    public class ChatbotService
    {
        private readonly AppDbContext _context;
        private readonly OpenAIClient _openAIClient;
        private readonly IConfiguration _configuration;
        private static readonly List<string> Saudacoes = new List<string> { "oi", "ola", "bom dia", "boa tarde", "boa noite", "e ai", "tudo bem" };

        public ChatbotService(AppDbContext context, OpenAIClient openAIClient, IConfiguration configuration)
        {
            _context = context;
            _openAIClient = openAIClient;
            _configuration = configuration;
        }

        // Função principal que processa a mensagem
        public async Task<(string Resposta, bool PedirConfirmacao, bool ChamadoAberto)> ProcessarMensagem(int chamadoId, string mensagem)
        {
            var msgLimpa = LimparMensagem(mensagem);

            // 1. Lógica de Saudação
            if (Saudacoes.Contains(msgLimpa))
            {
                return ("Olá! Tudo bem? Por favor, descreva seu problema em poucas palavras (ex: 'impressora não imprime' ou 'sem internet').", false, false);
            }

            // 2. Lógica de Busca no FAQ (Nível 1)
            var solucao = await EncontrarSolucao(msgLimpa);

            if (solucao != null)
            {
                // Achou no FAQ!
                var steps = solucao.Descricao.Split(new[] { "|STEP|" }, StringSplitOptions.RemoveEmptyEntries);
                var respostaFormatada = new System.Text.StringBuilder();

                if (steps.Length > 1)
                {
                    respostaFormatada.AppendLine("Encontrei uma solução! Por favor, tente estes passos em ordem:");
                    int i = 1;
                    foreach (var step in steps)
                    {
                        respostaFormatada.AppendLine($"*{i}. {step.Trim()}*"); // Formata com número e negrito (Markdown)
                        i++;
                    }
                    respostaFormatada.AppendLine("\nPor favor, tente *todos* os passos. Após tentar, me diga se o problema foi resolvido.");
                }
                else
                {
                    // Se for só 1 passo, manda direto
                    respostaFormatada.AppendLine(solucao.Descricao);
                    respostaFormatada.AppendLine("\nIsso resolveu o seu problema?");
                }

                return (respostaFormatada.ToString(), true, false); // true = pedir confirmação
            }

            // 3. Lógica de Fallback (Nível 2 - OpenAI)
            // Não achou no FAQ, vamos perguntar para a OpenAI
            var respostaOpenAI = await ChamarOpenAI(mensagem);

            if (!string.IsNullOrEmpty(respostaOpenAI))
            {
                // OpenAI respondeu!
                respostaOpenAI += "\n\nIsso resolveu o seu problema?";
                return (respostaOpenAI, true, false); // Pedir confirmação
            }

            // 4. Lógica de Desistência (Nível 3 - Abrir Chamado)
            // Nível 1 (FAQ) falhou E Nível 2 (OpenAI) falhou (deu erro ou veio vazia)
            var chamado = await _context.Chamados.FindAsync(chamadoId);
            if (chamado != null)
            {
                chamado.Status = "Aberto"; // O bot desiste e manda para o técnico
                await _context.SaveChangesAsync();
            }

            var msgTecnico = "Entendido. Tentei consultar minha base de dados mas não encontrei uma solução. \n\nEstou abrindo um chamado agora mesmo e um técnico de plantão irá te ajudar em breve.";
            return (msgTecnico, false, true); // true = chamado aberto
        }

        // Função que busca no banco de dados (Nível 1)
        private async Task<SolucaoIA> EncontrarSolucao(string mensagemLimpa)
        {
            var solucoes = await _context.SolucoesIA.ToListAsync();
            var palavrasChaveUsuario = new HashSet<string>(mensagemLimpa.Split(' '));

            SolucaoIA? melhorSolucao = null;
            int maxScore = 0;

            foreach (var solucao in solucoes)
            {
                var palavrasChaveFaq = new HashSet<string>(solucao.PalavrasChave.Replace(",", "").Split(' '));
                var score = palavrasChaveUsuario.Intersect(palavrasChaveFaq).Count();

                if (score > maxScore)
                {
                    maxScore = score;
                    melhorSolucao = solucao;
                }
            }

            // Só retorna uma solução se o "score" for minimamente bom
            return maxScore > 0 ? melhorSolucao : null;
        }

        // Limpa a mensagem para a busca
        private string LimparMensagem(string mensagem)
        {
            mensagem = mensagem.ToLower();
            mensagem = Regex.Replace(mensagem, @"[^\w\s]", ""); // Remove pontuação

            // Remove palavras de parada (stop words)
            var stopWords = new[] { "o", "a", "os", "as", "meu", "minha", "meus", "minhas", "de", "do", "da", "dos", "das", "em", "no", "na", "nos", "nas", "com", "por", "para", "que", "é", "está", "estou", "estao", "eu", "nao", "um", "uma" };

            var palavras = mensagem.Split(' ').Where(p => !stopWords.Contains(p) && p.Length > 1);
            return string.Join(" ", palavras);
        }

        // Função que chama a OpenAI (Nível 2)
        private async Task<string> ChamarOpenAI(string mensagemUsuario)
        {
            try
            {
                var modelName = _configuration["OpenAI:Model"] ?? "gpt-3.5-turbo";

                var chatCompletionsOptions = new ChatCompletionsOptions
                {
                    DeploymentName = modelName,
                    Messages =
                    {
                        // O "System Prompt" (Personalidade do Bot)
                        new ChatRequestSystemMessage("Você é um assistente de suporte de TI (Helpdesk). Seu nome é 'Agente 360'. Você deve responder de forma breve, técnica e direta. Divida as soluções em passos curtos. Nunca diga que você é uma IA da OpenAI. Se não souber a resposta, retorne apenas a palavra 'ERRO'."),
                        
                        // A mensagem do usuário
                        new ChatRequestUserMessage(mensagemUsuario)
                    },
                    Temperature = 0.2f, // Deixa a resposta menos "criativa" e mais direta
                    MaxTokens = 200
                };

                Response<ChatCompletions> response = await _openAIClient.GetChatCompletionsAsync(chatCompletionsOptions);

                var resposta = response.Value.Choices[0].Message.Content;

                if (string.IsNullOrEmpty(resposta) || resposta.Contains("ERRO"))
                {
                    return null; // OpenAI não soube responder
                }

                return resposta;
            }
            catch (Exception ex)
            {
                // Logar o erro (em um TCC, podemos só printar no console)
                Console.WriteLine($"Erro ao chamar OpenAI: {ex.Message}");
                return null; // Se der qualquer erro, retorna nulo e o bot Nível 3 assume
            }
        }
    }
}