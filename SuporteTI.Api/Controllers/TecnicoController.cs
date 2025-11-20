using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuporteTI.Api.Data;
using SuporteTI.Shared.Models.Dto;

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

        // Endpoint: Mudar status (CORRIGIDO)
        [HttpPut("status")]
        public async Task<IActionResult> UpdateStatus([FromBody] StatusUpdateRequest request)
        {
            // Usa o ID que veio no pacote, não o do Token
            var tecnico = await _context.Usuarios.FindAsync(request.TecnicoId);

            if (tecnico == null) return NotFound("Técnico não encontrado");

            tecnico.StatusDisponibilidade = request.NovoStatus;
            await _context.SaveChangesAsync();

            return Ok(new { novoStatus = tecnico.StatusDisponibilidade });
        }

        // Endpoint: Técnico aceita um chamado (CORRIGIDO para receber tecnicoId na URL)
        [HttpPut("aceitar-ticket/{id}")]
        public async Task<IActionResult> AceitarTicket(int id, [FromQuery] int tecnicoId)
        {
            var ticket = await _context.Chamados.FindAsync(id);
            if (ticket == null) return NotFound();

            if (ticket.Status != "Aberto")
            {
                return BadRequest("Este chamado não está mais aberto.");
            }

            ticket.TecnicoResponsavelId = tecnicoId;
            ticket.Status = "EmAndamento";
            await _context.SaveChangesAsync();

            return Ok();
        }

        // Endpoint: Ver tickets abertos
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
                    NomeSolicitante = c.UsuarioSolicitante.Nome
                })
                .ToListAsync();

            return Ok(tickets);
        }

        // Endpoint: Ver meus tickets em andamento
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
                    NomeSolicitante = c.UsuarioSolicitante.Nome
                })
                .ToListAsync();

            return Ok(tickets);
        }
    }
}