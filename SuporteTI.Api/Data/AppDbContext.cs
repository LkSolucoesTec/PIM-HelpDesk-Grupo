using Microsoft.EntityFrameworkCore;
using SuporteTI.Shared.Models;

namespace SuporteTI.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Nossas "Tabelas"
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Chamado> Chamados { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<HistoricoChat> HistoricoChats { get; set; }
        public DbSet<SolucaoIA> SolucoesIA { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- Configurações de Relacionamento (Importante!) ---

            // Relacionamento do Chamado com o Solicitante (Usuario)
            modelBuilder.Entity<Chamado>()
                .HasOne(c => c.UsuarioSolicitante)
                .WithMany(u => u.ChamadosAbertos)
                .HasForeignKey(c => c.UsuarioSolicitanteId)
                .OnDelete(DeleteBehavior.Restrict); // Não deletar usuário se tiver chamado

            // Relacionamento do Chamado com o Técnico (Usuario)
            modelBuilder.Entity<Chamado>()
                .HasOne(c => c.TecnicoResponsavel)
                .WithMany(u => u.ChamadosAtendidos)
                .HasForeignKey(c => c.TecnicoResponsavelId)
                .OnDelete(DeleteBehavior.SetNull); // Se o técnico for deletado, o chamado fica "sem técnico"

            // --- SEEDING (Semeando) - Seus dados iniciais ---

            // 1. Categorias (Expandido)
            var catHardware = new Categoria { Id = 1, Nome = "Hardware" };
            var catSoftware = new Categoria { Id = 2, Nome = "Software" };
            var catRede = new Categoria { Id = 3, Nome = "Rede e Internet" };
            var catContas = new Categoria { Id = 4, Nome = "Contas e Acessos" };
            var catGeral = new Categoria { Id = 5, Nome = "Geral" };
            modelBuilder.Entity<Categoria>().HasData(catHardware, catSoftware, catRede, catContas, catGeral);

            // 2. Usuários (Logins) - MESMA COISA DO BATCH 2
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario
                {
                    Id = 1,
                    Nome = "Usuário Padrão",
                    Email = "user", // Login
                    Senha = "usuario123", // Senha
                    Papel = "Usuario",
                    Setor = "Vendas",
                    Telefone = "11999999999",
                    StatusDisponibilidade = "Offline"
                },
                new Usuario
                {
                    Id = 2,
                    Nome = "Técnico Padrão",
                    Email = "tecnico", // Login
                    Senha = "tecnico123", // Senha
                    Papel = "Tecnico",
                    Especialidade = "Redes",
                    StatusDisponibilidade = "Online"
                },
                new Usuario
                {
                    Id = 3,
                    Nome = "Gerente Admin",
                    Email = "gerencia", // Login
                    Senha = "gerencia123", // Senha
                    Papel = "Gerencia",
                    Setor = "TI",
                    Telefone = "11888888888",
                    StatusDisponibilidade = "Online"
                }
            );

            // 3. Soluções (FAQ) - SEU ARQUIVO DE 50 PERGUNTAS
            modelBuilder.Entity<SolucaoIA>().HasData(
                new SolucaoIA { Id = 1, CategoriaId = 2, PalavrasChave = "lento, travando", Descricao = "Tente fechar todos os programas que não está usando.|STEP|Utilize a \"Limpeza de Disco\" para liberar espaço.|STEP|Reinicie o computador." },
                new SolucaoIA { Id = 2, CategoriaId = 3, PalavrasChave = "conectar à internet, sem internet, sem conexão, caiu", Descricao = "Verifique se o cabo de rede está bem conectado ao computador e ao roteador.|STEP|Tente reiniciar seu modem e roteador (desligue da tomada por 30 segundos e ligue novamente).|STEP|No caso de notebook, veja se o Wi-Fi está ativado." },
                new SolucaoIA { Id = 3, CategoriaId = 4, PalavrasChave = "esqueci minha senha, senha de e-mail, recuperar senha", Descricao = "Na tela de login do seu e-mail, clique no link \"Esqueci minha senha\" ou \"Recuperar senha\".|STEP|Siga as instruções para criar uma nova senha." },
                new SolucaoIA { Id = 4, CategoriaId = 1, PalavrasChave = "imprimir, impressora", Descricao = "Veja se a impressora está ligada e conectada ao computador.|STEP|Verifique se os níveis de tinta ou toner estão bons.|STEP|Tente reiniciar a impressora e o computador." },
                new SolucaoIA { Id = 5, CategoriaId = 2, PalavrasChave = "arquivos sumiram, perdi arquivo, arquivo desapareceu", Descricao = "Verifique a Lixeira do seu computador.|STEP|Use a barra de pesquisa para procurar pelo nome do arquivo.|STEP|Veja se você não moveu os arquivos para outra pasta sem querer." },
                new SolucaoIA { Id = 6, CategoriaId = 2, PalavrasChave = "programa travou, não responde", Descricao = "Pressione as teclas Ctrl + Alt + Del e escolha o \"Gerenciador de Tarefas\".|STEP|Na lista, encontre o programa travado, clique nele e depois em \"Finalizar tarefa\".|STEP|Se não funcionar, reinicie o computador." },
                new SolucaoIA { Id = 7, CategoriaId = 2, PalavrasChave = "travando com frequência, travamentos", Descricao = "Verifique se os programas e o sistema operacional estão atualizados.|STEP|Faça uma verificação completa com seu antivírus.|STEP|Veja se o computador não está esquentando demais." },
                new SolucaoIA { Id = 8, CategoriaId = 3, PalavrasChave = "acessar um site específico, site não abre", Descricao = "Tente acessar outros sites para ver se sua internet está funcionando.|STEP|Limpe o cache e os cookies do seu navegador.|STEP|Tente usar um navegador diferente (Chrome, Firefox, Edge)." },
                new SolucaoIA { Id = 9, CategoriaId = 2, PalavrasChave = "muito spam, e-mail spam", Descricao = "Marque sempre as mensagens indesejadas como \"Spam\" ou \"Lixo Eletrônico\".|STEP|Evite colocar seu e-mail em sites públicos.|STEP|Cancele a inscrição de newsletters que você não lê mais." },
                new SolucaoIA { Id = 10, CategoriaId = 2, PalavrasChave = "vírus, antivírus avisou", Descricao = "Siga as instruções do antivírus para remover ou colocar a ameaça em quarentena.|STEP|Depois, faça uma verificação completa no computador.|STEP|Mantenha seu antivírus sempre atualizado." },
                new SolucaoIA { Id = 11, CategoriaId = 1, PalavrasChave = "tela azul, blue screen", Descricao = "Anote o código de erro que aparece na tela, se possível.|STEP|Reinicie o computador.|STEP|Se acontecer de novo, procure por atualizações de drivers e do sistema operacional." },
                new SolucaoIA { Id = 12, CategoriaId = 3, PalavrasChave = "arquivo da rede, pasta da rede", Descricao = "Veja se você está conectado à rede da empresa (cabo ou Wi-Fi).|STEP|Verifique se você tem permissão para acessar aquela pasta.|STEP|Tente reiniciar o seu computador." },
                new SolucaoIA { Id = 13, CategoriaId = 1, PalavrasChave = "mouse parou, teclado parou", Descricao = "Verifique se o cabo está bem conectado na porta USB.|STEP|Se for sem fio, verifique as pilhas ou a bateria.|STEP|Tente conectar em outra porta USB." },
                new SolucaoIA { Id = 14, CategoriaId = 3, PalavrasChave = "videoconferência, reunião ruim, som ruim, imagem ruim", Descricao = "Feche outros programas e abas da internet que você não esteja usando.|STEP|Verifique as configurações de áudio e vídeo dentro do programa da reunião.|STEP|Tente usar um fone de ouvido com microfone." },
                new SolucaoIA { Id = 15, CategoriaId = 2, PalavrasChave = "instalar um programa, não instala", Descricao = "Clique com o botão direito no instalador e escolha \"Executar como administrador\".|STEP|Desative o antivírus temporariamente durante a instalação (lembre de ligar de novo depois).|STEP|Baixe o arquivo de instalação novamente, ele pode estar corrompido." },
                new SolucaoIA { Id = 16, CategoriaId = 1, PalavrasChave = "hd cheio, disco rígido cheio, sem espaço", Descricao = "Desinstale programas que você não usa mais.|STEP|Apague arquivos desnecessários, principalmente da pasta \"Downloads\".|STEP|Use a \"Limpeza de Disco\" para remover arquivos temporários." },
                new SolucaoIA { Id = 17, CategoriaId = 2, PalavrasChave = "data e hora erradas", Descricao = "Clique na data e hora na barra de tarefas e escolha \"Ajustar data/hora\".|STEP|Verifique se o fuso horário está correto.|STEP|Ative a opção para acertar o relógio automaticamente pela internet." },
                new SolucaoIA { Id = 18, CategoriaId = 2, PalavrasChave = "abrir pdf, abrir word, abrir docx", Descricao = "Verifique se você tem um programa instalado para abrir esse tipo de arquivo.|STEP|Para PDF, instale o Adobe Acrobat Reader.|STEP|Para DOCX, você pode usar o Word ou o LibreOffice." },
                new SolucaoIA { Id = 19, CategoriaId = 3, PalavrasChave = "wi-fi lenta, wifi lento", Descricao = "Desconecte da rede os aparelhos que não estão em uso.|STEP|Reinicie o seu roteador (desligue da tomada por 30 segundos).|STEP|Aproxime-se do roteador para ver se o sinal melhora." },
                new SolucaoIA { Id = 20, CategoriaId = 1, PalavrasChave = "notebook não liga", Descricao = "Verifique se o carregador está bem conectado no notebook e na tomada.|STEP|Veja se alguma luz acende quando você conecta o carregador.|STEP|Se a bateria for removível, tente tirar e ligar o notebook direto na tomada." },
                new SolucaoIA { Id = 21, CategoriaId = 1, PalavrasChave = "sem som, computador mudo", Descricao = "Verifique se o volume não está no mudo ou muito baixo.|STEP|Veja se o fone de ouvido ou as caixas de som estão bem conectados.|STEP|Reinicie o computador." },
                new SolucaoIA { Id = 22, CategoriaId = 3, PalavrasChave = "cadeado https, site https", Descricao = "Verifique se a data e a hora do seu computador estão corretas.|STEP|Limpe o cache e os cookies do seu navegador.|STEP|Veja se o seu navegador de internet está atualizado." },
                new SolucaoIA { Id = 23, CategoriaId = 1, PalavrasChave = "reinicia sozinho", Descricao = "Verifique se o computador não está superaquecendo.|STEP|Procure por atualizações do sistema operacional e de drivers.|STEP|Faça uma verificação completa com o antivírus." },
                new SolucaoIA { Id = 24, CategoriaId = 1, PalavrasChave = "tela piscando", Descricao = "Verifique se o cabo de vídeo (HDMI, VGA) está bem conectado no monitor e no computador.|STEP|Tente usar um cabo diferente.|STEP|Atualize o driver da placa de vídeo." },
                new SolucaoIA { Id = 25, CategoriaId = 4, PalavrasChave = "conta do trabalho, não entro na conta", Descricao = "Veja se a tecla Caps Lock (letras maiúsculas) não está ativada.|STEP|Tente usar a opção de redefinir a senha, se houver.|STEP|Entre em contato com o suporte de TI da sua empresa." },
                new SolucaoIA { Id = 26, CategoriaId = 3, PalavrasChave = "vpn não conecta", Descricao = "Verifique se a sua internet está funcionando normalmente.|STEP|Confira se seu usuário e senha da VPN estão corretos.|STEP|Reinicie o programa da VPN e o seu computador." },
                new SolucaoIA { Id = 27, CategoriaId = 3, PalavrasChave = "recurso da rede, impressora da rede, pasta da rede", Descricao = "Verifique se o equipamento (impressora, servidor) está ligado.|STEP|Confirme se você tem permissão para acessar esse recurso.|STEP|Peça ajuda ao administrador da rede." },
                new SolucaoIA { Id = 28, CategoriaId = 1, PalavrasChave = "muito barulho, barulhento", Descricao = "Verifique se as saídas de ar do computador não estão bloqueadas por poeira ou objetos.|STEP|Veja se o computador está em um local bem ventilado.|STEP|Se o barulho for muito alto, pode ser um problema no ventilador interno." },
                new SolucaoIA { Id = 29, CategoriaId = 1, PalavrasChave = "gravar cd, gravar dvd", Descricao = "Verifique se o CD/DVD que você está usando é do tipo gravável (CD-R, DVD-R).|STEP|Tente gravar em uma velocidade mais baixa.|STEP|Use um CD/DVD de outra marca para testar." },
                new SolucaoIA { Id = 30, CategoriaId = 1, PalavrasChave = "sem sinal, monitor sem sinal", Descricao = "Veja se o computador está realmente ligado.|STEP|Verifique se o cabo de vídeo está bem conectado no monitor e no computador.|STEP|Se tiver uma placa de vídeo dedicada, conecte o cabo nela, e não na placa-mãe." },
                new SolucaoIA { Id = 31, CategoriaId = 3, PalavrasChave = "serviço online, site fora do ar", Descricao = "Tente acessar outros sites para ver se sua internet está normal.|STEP|Verifique se o serviço não está fora do ar para todo mundo.|STEP|Limpe o cache e os cookies do seu navegador." },
                new SolucaoIA { Id = 32, CategoriaId = 1, PalavrasChave = "touchpad, mouse do notebook", Descricao = "Veja se você não desativou o touchpad sem querer por alguma tecla de atalho (geralmente Fn + uma das teclas F1 a F12).|STEP|Reinicie o notebook.|STEP|Conecte um mouse USB para ver se o problema é só no touchpad." },
                new SolucaoIA { Id = 33, CategoriaId = 2, PalavrasChave = "pop-ups, alertas falsos, malware", Descricao = "Não clique em nada nesses alertas.|STEP|Feche as janelas pop-up.|STEP|Faça uma verificação completa com seu antivírus e um programa anti-malware." },
                new SolucaoIA { Id = 34, CategoriaId = 2, PalavrasChave = "atualizações do windows, windows update", Descricao = "Verifique se você tem bastante espaço livre no disco (HD).|STEP|Reinicie o computador e tente atualizar de novo.|STEP|Use a \"Solução de Problemas do Windows Update\" nas configurações do Windows." },
                new SolucaoIA { Id = 35, CategoriaId = 3, PalavrasChave = "celular não conecta wi-fi", Descricao = "Confira se a senha do Wi-Fi está correta.|STEP|No seu celular, vá nas configurações de Wi-Fi, mande \"esquecer a rede\" e conecte-se de novo.|STEP|Reinicie seu celular." },
                new SolucaoIA { Id = 36, CategoriaId = 3, PalavrasChave = "nuvem, google drive, onedrive, não sincroniza", Descricao = "Verifique se sua internet está funcionando.|STEP|Veja se você ainda tem espaço de armazenamento livre na sua conta da nuvem.|STEP|Reinicie o programa de sincronização e o computador." },
                new SolucaoIA { Id = 37, CategoriaId = 2, PalavrasChave = "demora para ligar, boot lento", Descricao = "Desative programas desnecessários da inicialização do sistema (pelo Gerenciador de Tarefas na aba \"Inicializar\").|STEP|Faça uma verificação completa de vírus.|STEP|Verifique se há espaço livre no disco rígido." },
                new SolucaoIA { Id = 38, CategoriaId = 3, PalavrasChave = "área de trabalho remota, rdp", Descricao = "Verifique se o computador que você quer acessar está ligado e conectado à internet.|STEP|Confirme se a opção de \"Área de Trabalho Remota\" está ativada nesse computador.|STEP|Veja se você está digitando o nome ou IP do computador corretamente." },
                new SolucaoIA { Id = 39, CategoriaId = 2, PalavrasChave = "programa de e-mail, outlook, thunderbird", Descricao = "Verifique sua conexão com a internet.|STEP|Confira se a senha da sua conta de e-mail está correta nas configurações do programa.|STEP|Reinicie o programa de e-mail e o computador." },
                new SolucaoIA { Id = 40, CategoriaId = 1, PalavrasChave = "digitalizar, escanear, scanner", Descricao = "Verifique se o scanner está ligado e conectado ao computador.|STEP|Reinicie o scanner e o computador.|STEP|Tente usar o programa de digitalização que veio com o scanner." },
                new SolucaoIA { Id = 41, CategoriaId = 2, PalavrasChave = "navegador, sites estranhos, redirecionamento", Descricao = "Faça uma verificação completa com seu antivírus e um programa anti-malware.|STEP|Verifique as extensões instaladas no navegador e remova as suspeitas.|STEP|Restaure as configurações do navegador para o padrão." },
                new SolucaoIA { Id = 42, CategoriaId = 1, PalavrasChave = "bateria acaba rápido", Descricao = "Diminua o brilho da tela.|STEP|Feche programas que você não está usando.|STEP|Desligue o Wi-Fi e o Bluetooth se não precisar deles." },
                new SolucaoIA { Id = 43, CategoriaId = 3, PalavrasChave = "site interno, intranet", Descricao = "Verifique se você está conectado à rede da empresa (cabo, Wi-Fi ou VPN).|STEP|Tente acessar outros recursos internos para ver se o problema é só com esse site.|STEP|Fale com o suporte de TI da sua empresa." },
                new SolucaoIA { Id = 44, CategoriaId = 1, PalavrasChave = "pen drive, hd externo, não reconhece", Descricao = "Tente conectar o dispositivo em outra porta USB.|STEP|Verifique se ele funciona em outro computador.|STEP|Reinicie o seu computador com o dispositivo conectado." },
                new SolucaoIA { Id = 45, CategoriaId = 2, PalavrasChave = "programa específico lento", Descricao = "Feche outros programas que estejam abertos.|STEP|Verifique se há atualizações para esse programa.|STEP|Tente reiniciar o computador." },
                new SolucaoIA { Id = 46, CategoriaId = 2, PalavrasChave = "compartilhar tela, reunião", Descricao = "Verifique as configurações de privacidade do seu sistema para ver se o programa da reunião tem permissão para gravar a tela.|STEP|Saia e entre novamente na reunião.|STEP|Reinicie o programa da reunião." },
                new SolucaoIA { Id = 47, CategoriaId = 2, PalavrasChave = "arquivo corrompido, não abre", Descricao = "Se você tiver um backup, tente usar uma versão anterior do arquivo.|STEP|Veja se o programa que você usa para abrir o arquivo está atualizado.|STEP|Tente abrir o arquivo em outro computador." },
                new SolucaoIA { Id = 48, CategoriaId = 2, PalavrasChave = "formatação, documento bagunçado", Descricao = "Use os estilos de formatação padrão (Ex: Título 1, Normal) para organizar o texto.|STEP|Tente copiar todo o texto e colar em um documento novo \"sem formatação\".|STEP|Reinicie o programa (Word, Writer, etc)." },
                new SolucaoIA { Id = 49, CategoriaId = 4, PalavrasChave = "certificado digital", Descricao = "Verifique se o certificado não está vencido.|STEP|Certifique-se de que ele está instalado corretamente no computador ou navegador.|STEP|Tente reinstalar o certificado." },
                new SolucaoIA { Id = 50, CategoriaId = 1, PalavrasChave = "instalar impressora de rede", Descricao = "Veja se a impressora está ligada e conectada à mesma rede Wi-Fi que o seu computador.|STEP|No seu computador, vá em \"Configurações\" e depois \"Impressoras e Scanners\".|STEP|Clique em \"Adicionar uma impressora\" e veja se o computador a encontra sozinho." }
            );
        }
    }
    
}