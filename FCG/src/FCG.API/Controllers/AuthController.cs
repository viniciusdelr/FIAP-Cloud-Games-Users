using FCG.Infrastructure.Data;
using FCG.Application.DTOs;
using JWT_Example;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FCG.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtSettings _jwtSettings;
        private readonly DataContext _context;

        public AuthController(DataContext context, JwtSettings jwtSettings)
        {
            _jwtSettings = jwtSettings;
            _context = context;
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {
            var account = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == loginDto.Username);

            if (account == null)
                return BadRequest(new { mensagem = "Usuário não encontrado." });

            bool IncorrectPass = BCrypt.Net.BCrypt.Verify(loginDto.Password, account.Password);

            if (!IncorrectPass)
                return BadRequest(new { mensagem = "Senha incorreta." });

            var auth = new Auth.Auth(_jwtSettings);
            var token = auth.GerarAuth(account);

            return Ok(new
            {
                mensagem = "Login bem-sucedido",
                token = token
            });
        }
    }
}
