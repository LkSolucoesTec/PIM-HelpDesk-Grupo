using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuporteTI.Api.Data;
using SuporteTI.Shared.Models;

namespace SuporteTI.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        // TokenService foi removido
        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Senha))
            {
                return BadRequest("Email e Senha são obrigatórios.");
            }

            // 1. Acha o usuário no banco pelo Email (login)
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            // 2. Verifica se o usuário existe e se a senha está correta
            if (usuario == null || usuario.Senha != request.Senha)
            {
                return Unauthorized("Usuário ou senha inválidos.");
            }

            // 3. SUCESSO! Retorna os dados do usuário (sem token)
            return Ok(new LoginResponse
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Papel = usuario.Papel
                // A propriedade "Token" ficará vazia (null)
            });
        }
    }
}