using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuporteTI.Api.Data;
using SuporteTI.Shared.Models.Dto;
using System.Security.Claims;

namespace SuporteTI.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class TecnicoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TecnicoController(AppDbContext context)
        {
            _context = context;
        }

        // Endpoint 1: Ver a fila de chamados "Abertos"
        [HttpGet("tickets-abertos")]
        public async Task<IActionResult> GetTicketsAbertos()
        {
            var tickets = await _context.Chamados
                .Include(c => c.UsuarioSolicitante)
                .Where(c => c.Status == "Aberto")
                .OrderBy(c => c.DataAbertura)
                .Select(c => new TicketListDto
                {
                    Id = c.Id,
                    Titulo = c.Titulo,
                    Status = c.Status,
                    Prioridade = c.Prioridade,
                    DataAbertura = c.DataAbertura,
                    NomeSolicitante = c.UsuarioSolicitante != null ? c.UsuarioSolicitante.Nome : "Desconhecido"
                })
                .ToListAsync();

            return Ok(tickets);
        }

        // Endpoint 2: Ver o detalhe de um chamado (CORRIGIDO)
        [HttpGet("ticket/{id}")]
        public async Task<IActionResult> GetTicketDetail(int id)
        {
            // CORREÇÃO: Simplificamos a query. Tiramos o OrderBy do Include para evitar erros de tradução SQL.
            var ticket = await _context.Chamados
                .Include(c => c.UsuarioSolicitante)
                .Include(c => c.Historico)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (ticket == null) return NotFound("Chamado não encontrado no banco de dados.");

            // Ordenação feita em memória (mais seguro)
            var historicoOrdenado = ticket.Historico.OrderBy(h => h.Timestamp).ToList();

            var ticketDto = new TicketDetailDto
            {
                Id = ticket.Id,
                Titulo = ticket.Titulo,
                DescricaoInicial = ticket.DescricaoInicial,
                Status = ticket.Status,
                Prioridade = ticket.Prioridade,
                DataAbertura = ticket.DataAbertura,
                // Null-checks para evitar quebrar se o usuário foi deletado
                NomeSolicitante = ticket.UsuarioSolicitante?.Nome ?? "Usuário Removido",
                SetorSolicitante = ticket.UsuarioSolicitante?.Setor ?? "N/A",
                TelefoneSolicitante = ticket.UsuarioSolicitante?.Telefone ?? "N/A",

                Historico = historicoOrdenado.Select(h => new HistoricoChatDto
                {
                    Remetente = h.Remetente,
                    Conteudo = h.Conteudo,
                    Timestamp = h.Timestamp,
                    Resolveu = h.Resolveu
                }).ToList()
            };

            return Ok(ticketDto);
        }

        // Endpoint 3: Técnico aceita um chamado
        [HttpPut("aceitar-ticket/{id}")]
        public async Task<IActionResult> AceitarTicket(int id)
        {
            var ticket = await _context.Chamados.FindAsync(id);
            if (ticket == null) return NotFound();

            if (ticket.Status != "Aberto")
            {
                return BadRequest("Este chamado não está mais aberto.");
            }

            // Tenta pegar o ID do token, se falhar, usa um ID fixo ou do parâmetro (para facilitar testes)
            int tecnicoId = 0;
            var claimId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (claimId != null)
            {
                tecnicoId = int.Parse(claimId);
            }
            else
            {
                // Fallback para testes se estiver sem autenticação
                // Tenta achar o primeiro técnico disponível
                var tecnicoPadrao = await _context.Usuarios.FirstOrDefaultAsync(u => u.Papel == "Tecnico");
                if (tecnicoPadrao != null) tecnicoId = tecnicoPadrao.Id;
            }

            ticket.TecnicoResponsavelId = tecnicoId;
            ticket.Status = "EmAndamento";
            await _context.SaveChangesAsync();

            return Ok();
        }

        // Endpoint 4: Técnico fecha um chamado
        [HttpPut("fechar-ticket")]
        public async Task<IActionResult> FecharTicket([FromBody] CloseTicketRequest request)
        {
            var ticket = await _context.Chamados.FindAsync(request.ChamadoId);
            if (ticket == null) return NotFound();

            ticket.Status = "ResolvidoTecnico";
            ticket.ObservacaoTecnica = request.ObservacaoTecnica;
            ticket.DataFechamento = DateTime.Now;

            await _context.SaveChangesAsync();
            return Ok();
        }

        // Endpoint 5: Mudar status
        [HttpPut("status")]
        public async Task<IActionResult> UpdateStatus([FromBody] StatusUpdateRequest request)
        {
            int tecnicoId = request.TecnicoId;

            // Se não veio ID no request, tenta pegar do token
            if (tecnicoId == 0)
            {
                var claimId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (claimId != null) tecnicoId = int.Parse(claimId);
            }

            var tecnico = await _context.Usuarios.FindAsync(tecnicoId);
            if (tecnico == null) return NotFound("Técnico não encontrado");

            tecnico.StatusDisponibilidade = request.NovoStatus;
            await _context.SaveChangesAsync();

            return Ok(new { novoStatus = tecnico.StatusDisponibilidade });
        }

        // Endpoint 6: Ver Meus chamados
        [HttpGet("tickets-em-andamento/{tecnicoId}")]
        public async Task<IActionResult> GetTicketsEmAndamento(int tecnicoId)
        {
            var tickets = await _context.Chamados
                .Include(c => c.UsuarioSolicitante)
                .Where(c => c.Status == "EmAndamento" && c.TecnicoResponsavelId == tecnicoId)
                .OrderBy(c => c.DataAbertura)
                .Select(c => new TicketListDto
                {
                    Id = c.Id,
                    Titulo = c.Titulo,
                    Status = c.Status,
                    Prioridade = c.Prioridade,
                    DataAbertura = c.DataAbertura,
                    NomeSolicitante = c.UsuarioSolicitante != null ? c.UsuarioSolicitante.Nome : "Desconhecido"
                })
                .ToListAsync();

            return Ok(tickets);
        }

        // Endpoint 7: Ver chamados resolvidos pela IA
        [HttpGet("tickets-resolvidos-ia")]
        public async Task<IActionResult> GetTicketsResolvidosIA()
        {
            var tickets = await _context.Chamados
                .Include(c => c.UsuarioSolicitante)
                .Where(c => c.Status == "ResolvidoIA")
                .OrderByDescending(c => c.DataFechamento)
                .Select(c => new TicketListDto
                {
                    Id = c.Id,
                    Titulo = c.Titulo,
                    Status = c.Status,
                    Prioridade = c.Prioridade,
                    DataAbertura = c.DataAbertura,
                    NomeSolicitante = c.UsuarioSolicitante != null ? c.UsuarioSolicitante.Nome : "Desconhecido"
                })
                .ToListAsync();

            return Ok(tickets);
        }
    }
}