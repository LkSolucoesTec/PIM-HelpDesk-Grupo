using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SuporteTI.Api.Migrations
{
    /// <inheritdoc />
    public partial class FullFAQ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Senha = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Papel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Setor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telefone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Especialidade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusDisponibilidade = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SolucoesIA",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PalavrasChave = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoriaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolucoesIA", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SolucoesIA_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Chamados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescricaoInicial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prioridade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataAbertura = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataFechamento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ObservacaoTecnica = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UsuarioSolicitanteId = table.Column<int>(type: "int", nullable: false),
                    TecnicoResponsavelId = table.Column<int>(type: "int", nullable: true),
                    CategoriaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chamados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chamados_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Chamados_Usuarios_TecnicoResponsavelId",
                        column: x => x.TecnicoResponsavelId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Chamados_Usuarios_UsuarioSolicitanteId",
                        column: x => x.UsuarioSolicitanteId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HistoricoChats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChamadoId = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Remetente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Conteudo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Resolveu = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoricoChats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoricoChats_Chamados_ChamadoId",
                        column: x => x.ChamadoId,
                        principalTable: "Chamados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "Id", "Nome" },
                values: new object[,]
                {
                    { 1, "Hardware" },
                    { 2, "Software" },
                    { 3, "Rede e Internet" },
                    { 4, "Contas e Acessos" },
                    { 5, "Geral" }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Email", "Especialidade", "Nome", "Papel", "Senha", "Setor", "StatusDisponibilidade", "Telefone" },
                values: new object[,]
                {
                    { 1, "user", null, "Usuário Padrão", "Usuario", "usuario123", "Vendas", "Offline", "11999999999" },
                    { 2, "tecnico", "Redes", "Técnico Padrão", "Tecnico", "tecnico123", null, "Online", null },
                    { 3, "gerencia", null, "Gerente Admin", "Gerencia", "gerencia123", "TI", "Online", "11888888888" }
                });

            migrationBuilder.InsertData(
                table: "SolucoesIA",
                columns: new[] { "Id", "CategoriaId", "Descricao", "PalavrasChave" },
                values: new object[,]
                {
                    { 1, 2, "Tente fechar todos os programas que não está usando.|STEP|Utilize a \"Limpeza de Disco\" para liberar espaço.|STEP|Reinicie o computador.", "lento, travando" },
                    { 2, 3, "Verifique se o cabo de rede está bem conectado ao computador e ao roteador.|STEP|Tente reiniciar seu modem e roteador (desligue da tomada por 30 segundos e ligue novamente).|STEP|No caso de notebook, veja se o Wi-Fi está ativado.", "conectar à internet, sem internet, sem conexão, caiu" },
                    { 3, 4, "Na tela de login do seu e-mail, clique no link \"Esqueci minha senha\" ou \"Recuperar senha\".|STEP|Siga as instruções para criar uma nova senha.", "esqueci minha senha, senha de e-mail, recuperar senha" },
                    { 4, 1, "Veja se a impressora está ligada e conectada ao computador.|STEP|Verifique se os níveis de tinta ou toner estão bons.|STEP|Tente reiniciar a impressora e o computador.", "imprimir, impressora" },
                    { 5, 2, "Verifique a Lixeira do seu computador.|STEP|Use a barra de pesquisa para procurar pelo nome do arquivo.|STEP|Veja se você não moveu os arquivos para outra pasta sem querer.", "arquivos sumiram, perdi arquivo, arquivo desapareceu" },
                    { 6, 2, "Pressione as teclas Ctrl + Alt + Del e escolha o \"Gerenciador de Tarefas\".|STEP|Na lista, encontre o programa travado, clique nele e depois em \"Finalizar tarefa\".|STEP|Se não funcionar, reinicie o computador.", "programa travou, não responde" },
                    { 7, 2, "Verifique se os programas e o sistema operacional estão atualizados.|STEP|Faça uma verificação completa com seu antivírus.|STEP|Veja se o computador não está esquentando demais.", "travando com frequência, travamentos" },
                    { 8, 3, "Tente acessar outros sites para ver se sua internet está funcionando.|STEP|Limpe o cache e os cookies do seu navegador.|STEP|Tente usar um navegador diferente (Chrome, Firefox, Edge).", "acessar um site específico, site não abre" },
                    { 9, 2, "Marque sempre as mensagens indesejadas como \"Spam\" ou \"Lixo Eletrônico\".|STEP|Evite colocar seu e-mail em sites públicos.|STEP|Cancele a inscrição de newsletters que você não lê mais.", "muito spam, e-mail spam" },
                    { 10, 2, "Siga as instruções do antivírus para remover ou colocar a ameaça em quarentena.|STEP|Depois, faça uma verificação completa no computador.|STEP|Mantenha seu antivírus sempre atualizado.", "vírus, antivírus avisou" },
                    { 11, 1, "Anote o código de erro que aparece na tela, se possível.|STEP|Reinicie o computador.|STEP|Se acontecer de novo, procure por atualizações de drivers e do sistema operacional.", "tela azul, blue screen" },
                    { 12, 3, "Veja se você está conectado à rede da empresa (cabo ou Wi-Fi).|STEP|Verifique se você tem permissão para acessar aquela pasta.|STEP|Tente reiniciar o seu computador.", "arquivo da rede, pasta da rede" },
                    { 13, 1, "Verifique se o cabo está bem conectado na porta USB.|STEP|Se for sem fio, verifique as pilhas ou a bateria.|STEP|Tente conectar em outra porta USB.", "mouse parou, teclado parou" },
                    { 14, 3, "Feche outros programas e abas da internet que você não esteja usando.|STEP|Verifique as configurações de áudio e vídeo dentro do programa da reunião.|STEP|Tente usar um fone de ouvido com microfone.", "videoconferência, reunião ruim, som ruim, imagem ruim" },
                    { 15, 2, "Clique com o botão direito no instalador e escolha \"Executar como administrador\".|STEP|Desative o antivírus temporariamente durante a instalação (lembre de ligar de novo depois).|STEP|Baixe o arquivo de instalação novamente, ele pode estar corrompido.", "instalar um programa, não instala" },
                    { 16, 1, "Desinstale programas que você não usa mais.|STEP|Apague arquivos desnecessários, principalmente da pasta \"Downloads\".|STEP|Use a \"Limpeza de Disco\" para remover arquivos temporários.", "hd cheio, disco rígido cheio, sem espaço" },
                    { 17, 2, "Clique na data e hora na barra de tarefas e escolha \"Ajustar data/hora\".|STEP|Verifique se o fuso horário está correto.|STEP|Ative a opção para acertar o relógio automaticamente pela internet.", "data e hora erradas" },
                    { 18, 2, "Verifique se você tem um programa instalado para abrir esse tipo de arquivo.|STEP|Para PDF, instale o Adobe Acrobat Reader.|STEP|Para DOCX, você pode usar o Word ou o LibreOffice.", "abrir pdf, abrir word, abrir docx" },
                    { 19, 3, "Desconecte da rede os aparelhos que não estão em uso.|STEP|Reinicie o seu roteador (desligue da tomada por 30 segundos).|STEP|Aproxime-se do roteador para ver se o sinal melhora.", "wi-fi lenta, wifi lento" },
                    { 20, 1, "Verifique se o carregador está bem conectado no notebook e na tomada.|STEP|Veja se alguma luz acende quando você conecta o carregador.|STEP|Se a bateria for removível, tente tirar e ligar o notebook direto na tomada.", "notebook não liga" },
                    { 21, 1, "Verifique se o volume não está no mudo ou muito baixo.|STEP|Veja se o fone de ouvido ou as caixas de som estão bem conectados.|STEP|Reinicie o computador.", "sem som, computador mudo" },
                    { 22, 3, "Verifique se a data e a hora do seu computador estão corretas.|STEP|Limpe o cache e os cookies do seu navegador.|STEP|Veja se o seu navegador de internet está atualizado.", "cadeado https, site https" },
                    { 23, 1, "Verifique se o computador não está superaquecendo.|STEP|Procure por atualizações do sistema operacional e de drivers.|STEP|Faça uma verificação completa com o antivírus.", "reinicia sozinho" },
                    { 24, 1, "Verifique se o cabo de vídeo (HDMI, VGA) está bem conectado no monitor e no computador.|STEP|Tente usar um cabo diferente.|STEP|Atualize o driver da placa de vídeo.", "tela piscando" },
                    { 25, 4, "Veja se a tecla Caps Lock (letras maiúsculas) não está ativada.|STEP|Tente usar a opção de redefinir a senha, se houver.|STEP|Entre em contato com o suporte de TI da sua empresa.", "conta do trabalho, não entro na conta" },
                    { 26, 3, "Verifique se a sua internet está funcionando normalmente.|STEP|Confira se seu usuário e senha da VPN estão corretos.|STEP|Reinicie o programa da VPN e o seu computador.", "vpn não conecta" },
                    { 27, 3, "Verifique se o equipamento (impressora, servidor) está ligado.|STEP|Confirme se você tem permissão para acessar esse recurso.|STEP|Peça ajuda ao administrador da rede.", "recurso da rede, impressora da rede, pasta da rede" },
                    { 28, 1, "Verifique se as saídas de ar do computador não estão bloqueadas por poeira ou objetos.|STEP|Veja se o computador está em um local bem ventilado.|STEP|Se o barulho for muito alto, pode ser um problema no ventilador interno.", "muito barulho, barulhento" },
                    { 29, 1, "Verifique se o CD/DVD que você está usando é do tipo gravável (CD-R, DVD-R).|STEP|Tente gravar em uma velocidade mais baixa.|STEP|Use um CD/DVD de outra marca para testar.", "gravar cd, gravar dvd" },
                    { 30, 1, "Veja se o computador está realmente ligado.|STEP|Verifique se o cabo de vídeo está bem conectado no monitor e no computador.|STEP|Se tiver uma placa de vídeo dedicada, conecte o cabo nela, e não na placa-mãe.", "sem sinal, monitor sem sinal" },
                    { 31, 3, "Tente acessar outros sites para ver se sua internet está normal.|STEP|Verifique se o serviço não está fora do ar para todo mundo.|STEP|Limpe o cache e os cookies do seu navegador.", "serviço online, site fora do ar" },
                    { 32, 1, "Veja se você não desativou o touchpad sem querer por alguma tecla de atalho (geralmente Fn + uma das teclas F1 a F12).|STEP|Reinicie o notebook.|STEP|Conecte um mouse USB para ver se o problema é só no touchpad.", "touchpad, mouse do notebook" },
                    { 33, 2, "Não clique em nada nesses alertas.|STEP|Feche as janelas pop-up.|STEP|Faça uma verificação completa com seu antivírus e um programa anti-malware.", "pop-ups, alertas falsos, malware" },
                    { 34, 2, "Verifique se você tem bastante espaço livre no disco (HD).|STEP|Reinicie o computador e tente atualizar de novo.|STEP|Use a \"Solução de Problemas do Windows Update\" nas configurações do Windows.", "atualizações do windows, windows update" },
                    { 35, 3, "Confira se a senha do Wi-Fi está correta.|STEP|No seu celular, vá nas configurações de Wi-Fi, mande \"esquecer a rede\" e conecte-se de novo.|STEP|Reinicie seu celular.", "celular não conecta wi-fi" },
                    { 36, 3, "Verifique se sua internet está funcionando.|STEP|Veja se você ainda tem espaço de armazenamento livre na sua conta da nuvem.|STEP|Reinicie o programa de sincronização e o computador.", "nuvem, google drive, onedrive, não sincroniza" },
                    { 37, 2, "Desative programas desnecessários da inicialização do sistema (pelo Gerenciador de Tarefas na aba \"Inicializar\").|STEP|Faça uma verificação completa de vírus.|STEP|Verifique se há espaço livre no disco rígido.", "demora para ligar, boot lento" },
                    { 38, 3, "Verifique se o computador que você quer acessar está ligado e conectado à internet.|STEP|Confirme se a opção de \"Área de Trabalho Remota\" está ativada nesse computador.|STEP|Veja se você está digitando o nome ou IP do computador corretamente.", "área de trabalho remota, rdp" },
                    { 39, 2, "Verifique sua conexão com a internet.|STEP|Confira se a senha da sua conta de e-mail está correta nas configurações do programa.|STEP|Reinicie o programa de e-mail e o computador.", "programa de e-mail, outlook, thunderbird" },
                    { 40, 1, "Verifique se o scanner está ligado e conectado ao computador.|STEP|Reinicie o scanner e o computador.|STEP|Tente usar o programa de digitalização que veio com o scanner.", "digitalizar, escanear, scanner" },
                    { 41, 2, "Faça uma verificação completa com seu antivírus e um programa anti-malware.|STEP|Verifique as extensões instaladas no navegador e remova as suspeitas.|STEP|Restaure as configurações do navegador para o padrão.", "navegador, sites estranhos, redirecionamento" },
                    { 42, 1, "Diminua o brilho da tela.|STEP|Feche programas que você não está usando.|STEP|Desligue o Wi-Fi e o Bluetooth se não precisar deles.", "bateria acaba rápido" },
                    { 43, 3, "Verifique se você está conectado à rede da empresa (cabo, Wi-Fi ou VPN).|STEP|Tente acessar outros recursos internos para ver se o problema é só com esse site.|STEP|Fale com o suporte de TI da sua empresa.", "site interno, intranet" },
                    { 44, 1, "Tente conectar o dispositivo em outra porta USB.|STEP|Verifique se ele funciona em outro computador.|STEP|Reinicie o seu computador com o dispositivo conectado.", "pen drive, hd externo, não reconhece" },
                    { 45, 2, "Feche outros programas que estejam abertos.|STEP|Verifique se há atualizações para esse programa.|STEP|Tente reiniciar o computador.", "programa específico lento" },
                    { 46, 2, "Verifique as configurações de privacidade do seu sistema para ver se o programa da reunião tem permissão para gravar a tela.|STEP|Saia e entre novamente na reunião.|STEP|Reinicie o programa da reunião.", "compartilhar tela, reunião" },
                    { 47, 2, "Se você tiver um backup, tente usar uma versão anterior do arquivo.|STEP|Veja se o programa que você usa para abrir o arquivo está atualizado.|STEP|Tente abrir o arquivo em outro computador.", "arquivo corrompido, não abre" },
                    { 48, 2, "Use os estilos de formatação padrão (Ex: Título 1, Normal) para organizar o texto.|STEP|Tente copiar todo o texto e colar em um documento novo \"sem formatação\".|STEP|Reinicie o programa (Word, Writer, etc).", "formatação, documento bagunçado" },
                    { 49, 4, "Verifique se o certificado não está vencido.|STEP|Certifique-se de que ele está instalado corretamente no computador ou navegador.|STEP|Tente reinstalar o certificado.", "certificado digital" },
                    { 50, 1, "Veja se a impressora está ligada e conectada à mesma rede Wi-Fi que o seu computador.|STEP|No seu computador, vá em \"Configurações\" e depois \"Impressoras e Scanners\".|STEP|Clique em \"Adicionar uma impressora\" e veja se o computador a encontra sozinho.", "instalar impressora de rede" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chamados_CategoriaId",
                table: "Chamados",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Chamados_TecnicoResponsavelId",
                table: "Chamados",
                column: "TecnicoResponsavelId");

            migrationBuilder.CreateIndex(
                name: "IX_Chamados_UsuarioSolicitanteId",
                table: "Chamados",
                column: "UsuarioSolicitanteId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoricoChats_ChamadoId",
                table: "HistoricoChats",
                column: "ChamadoId");

            migrationBuilder.CreateIndex(
                name: "IX_SolucoesIA_CategoriaId",
                table: "SolucoesIA",
                column: "CategoriaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistoricoChats");

            migrationBuilder.DropTable(
                name: "SolucoesIA");

            migrationBuilder.DropTable(
                name: "Chamados");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
