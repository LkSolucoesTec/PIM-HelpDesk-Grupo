using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuporteTI.Api.Data;
using SuporteTI.Api.Services;
using SuporteTI.Shared.Models;
using SuporteTI.Shared.Models.Dto;

namespace SuporteTI.Api.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class ChatbotController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ChatbotService _chatbotService;

        public ChatbotController(AppDbContext context, ChatbotService chatbotService)
        {
            _context = context;
            _chatbotService = chatbotService;
        }

        [HttpPost("mensagem")]
        public async Task<IActionResult> EnviarMensagem([FromBody] ChatRequest request)
        {
            var chamadoId = request.ChamadoId;
            var usuarioId = request.UsuarioId;
            var mensagem = request.Mensagem;
            var lowerMensagem = mensagem.ToLower(); // Usar para a lógica de prioridade

            // 1. Processa a mensagem no Cérebro (ChatbotService) PRIMEIRO
            var (respostaBot, pedirConfirmacao, chamadoAberto) = await _chatbotService.ProcessarMensagem(chamadoId, mensagem);

            // 2. Lógica de criação do Chamado (Agora é inteligente)
            if (chamadoId == 0)
            {
                // Se não for uma saudação (ou seja, é um problema real) E o bot não resolveu na primeira, criamos o ticket
                bool eProblema = !respostaBot.Contains("Olá! Tudo bem?");

                if (eProblema)
                {
                    var novoChamado = new Chamado
                    {
                        UsuarioSolicitanteId = usuarioId,
                        // CORREÇÃO: O Título agora é a mensagem do problema!
                        Titulo = $"Chat: {mensagem.Substring(0, Math.Min(50, mensagem.Length))}",
                        DescricaoInicial = mensagem,
                        Status = "BotAtendimento",

                        // --- LÓGICA DE PRIORIDADE (Batch 87) ---
                        Prioridade = (lowerMensagem.Contains("virus")) ? "Alta" :
                                     (lowerMensagem.Contains("software") || lowerMensagem.Contains("programa")) ? "Media" :
                                     (lowerMensagem.Contains("impressora") || lowerMensagem.Contains("mouse") || lowerMensagem.Contains("teclado") || lowerMensagem.Contains("hardware")) ? "Baixa" :
                                     "Baixa" // Padrão
                    };
                    _context.Chamados.Add(novoChamado);
                    await _context.SaveChangesAsync();
                    chamadoId = novoChamado.Id; // Atualiza o ID
                }
            }

            // 3. Se um chamado já existe (ou acabou de ser criado), salva o histórico
            if (chamadoId != 0)
            {
                var historicoUsuario = new HistoricoChat
                {
                    ChamadoId = chamadoId,
                    Remetente = "Usuario",
                    Conteudo = mensagem
                };
                _context.HistoricoChats.Add(historicoUsuario);

                var historicoBot = new HistoricoChat
                {
                    ChamadoId = chamadoId,
                    Remetente = "Bot",
                    Conteudo = respostaBot
                };
                _context.HistoricoChats.Add(historicoBot);
                await _context.SaveChangesAsync();
            }

            // 4. Retorna a resposta para o Front-end
            return Ok(new ChatResponse
            {
                ChamadoId = chamadoId,
                RespostaBot = respostaBot,
                PedirConfirmacao = pedirConfirmacao,
                ChamadoAberto = chamadoAberto
            });
        }

        [HttpPost("confirmar")]
        public async Task<IActionResult> ConfirmarResolucao([FromBody] ConfirmacaoRequest request)
        {
            var chamado = await _context.Chamados.FindAsync(request.ChamadoId);
            if (chamado == null) return NotFound("Chamado não encontrado");

            string respostaBot;

            if (request.Resolveu) // Usuário clicou [Sim]
            {
                chamado.Status = "ResolvidoIA";
                chamado.DataFechamento = DateTime.Now;
                respostaBot = "Perfeito! Fico feliz em ajudar. Estou encerrando este chamado. Tenha um ótimo dia!";
            }
            else // Usuário clicou [Não]
            {
                chamado.Status = "Aberto"; // Manda para o técnico
                respostaBot = "Entendido. Estou transferindo seu caso para um técnico agora mesmo. Em breve ele entrará em contato.";
            }

            _context.HistoricoChats.Add(new HistoricoChat
            {
                ChamadoId = request.ChamadoId,
                Remetente = "Bot",
                Conteudo = respostaBot,
                Resolveu = request.Resolveu
            });
            await _context.SaveChangesAsync();

            return Ok(new ChatResponse
            {
                ChamadoId = request.ChamadoId,
                RespostaBot = respostaBot,
                PedirConfirmacao = false,
                ChamadoAberto = !request.Resolveu // Se não resolveu, o chamado fica aberto
            });
        }
    }
}