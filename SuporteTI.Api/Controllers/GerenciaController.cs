using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuporteTI.Api.Data;
using SuporteTI.Shared.Models;
using SuporteTI.Shared.Models.Dto;
using System.Security.Claims;

namespace SuporteTI.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GerenciaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GerenciaController(AppDbContext context)
        {
            _context = context;
        }

        // Endpoint 1: Pegar as Métricas do Dashboard
        [HttpGet("dashboard-metrics")]
        public async Task<IActionResult> GetDashboardMetrics()
        {
            var chamados = await _context.Chamados.ToListAsync();
            var tecnicos = await _context.Usuarios
                .Where(u => u.Papel == "Tecnico")
                .Select(u => new TecnicoStatusDto
                {
                    NomeTecnico = u.Nome,
                    StatusDisponibilidade = u.StatusDisponibilidade
                })
                .ToListAsync();

            var chamadosFechados = chamados
                .Where(c => c.Status == "ResolvidoIA" || c.Status == "ResolvidoTecnico")
                .ToList();

            double tempoMedioHoras = 0;
            if (chamadosFechados.Any())
            {
                tempoMedioHoras = chamadosFechados
                    .Average(c => (c.DataFechamento.Value - c.DataAbertura).TotalHours);
            }

            var metrics = new DashboardMetricsDto
            {
                TotalChamados = chamados.Count,
                TotalAbertos = chamados.Count(c => c.Status == "Aberto"),
                TotalEmAndamento = chamados.Count(c => c.Status == "EmAndamento"),
                TotalResolvidosIA = chamados.Count(c => c.Status == "ResolvidoIA"),
                TotalResolvidosTecnico = chamados.Count(c => c.Status == "ResolvidoTecnico"),
                TempoMedioResolucaoHoras = Math.Round(tempoMedioHoras, 2),
                StatusTecnicos = tecnicos
            };

            return Ok(metrics);
        }

        [HttpGet("categorias")]
        public async Task<IActionResult> GetCategorias()
        {
            return Ok(await _context.Categorias.ToListAsync());
        }

        [HttpPost("categorias")]
        public async Task<IActionResult> CreateCategoria([FromBody] Categoria categoria)
        {
            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();
            return Ok(categoria);
        }

        [HttpDelete("categorias/{id}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            var cat = await _context.Categorias.FindAsync(id);
            if (cat == null) return NotFound();

            _context.Categorias.Remove(cat);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Endpoint 2: Listar todos os Usuários (Admin)
        [HttpGet("usuarios")]
        public async Task<IActionResult> GetUsuarios()
        {
            var usuarios = await _context.Usuarios
                .Select(u => new UserListDto
                {
                    Id = u.Id,
                    Nome = u.Nome,
                    Email = u.Email,
                    Papel = u.Papel,
                    Setor = u.Setor,
                    Especialidade = u.Especialidade,
                    StatusDisponibilidade = u.StatusDisponibilidade
                })
                .OrderBy(u => u.Nome)
                .ToListAsync();

            return Ok(usuarios);
        }

        // Endpoint 3: Criar um novo Usuário ou Técnico
        [HttpPost("usuarios")]
        public async Task<IActionResult> CreateUsuario([FromBody] UserCreateDto dto)
        {
            var emailExists = await _context.Usuarios.AnyAsync(u => u.Email == dto.Email);
            if (emailExists)
            {
                return BadRequest("O email (login) já está em uso.");
            }

            var novoUsuario = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Senha = dto.Senha, // Lembrete: Senha em texto plano apenas para o TCC
                Papel = dto.Papel,
                Setor = dto.Setor,
                Telefone = dto.Telefone,
                Especialidade = dto.Especialidade,
                StatusDisponibilidade = (dto.Papel == "Tecnico") ? "Online" : "Offline"
            };

            _context.Usuarios.Add(novoUsuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsuario), new { id = novoUsuario.Id }, novoUsuario);
        }

        // Endpoint 4: Pegar um usuário específico (necessário para o "CreatedAtAction")
        [HttpGet("usuarios/{id}")]
        public async Task<IActionResult> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();
            return Ok(usuario); // Retorna o modelo completo
        }

        // Endpoint 5: Deletar um usuário
        [HttpDelete("usuarios/{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();

            // Lógica de segurança para não se auto-deletar
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (usuario.Id.ToString() == currentUserId)
            {
                return BadRequest("Você não pode deletar a si mesmo.");
            }

            // TODO: Adicionar lógica para reatribuir chamados se deletar um técnico.
            // Por simplicidade do TCC, vamos apenas deletar.

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent(); // Sucesso
        }
    }
}