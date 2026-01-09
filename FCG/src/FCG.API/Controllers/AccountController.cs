using FCG.Infrastructure.Data;
using FCG.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using FCG.Application.DTOs;
using MassTransit;
using FCG.API.Events;

namespace FCG.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IPublishEndpoint _publishEndpoint;
        public AccountController(AppDbContext context, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] AccountDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                return BadRequest(new { mensagem = "E-mail já cadastrado." });

            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
                return BadRequest(new { mensagem = "Username já cadastrado." });

            if (!Regex.IsMatch(dto.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return BadRequest(new { mensagem = "Formato de e-mail inválido." });

            if (!Regex.IsMatch(dto.Password, @"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$"))
                return BadRequest(new { mensagem = "A senha deve ter no mínimo 8 caracteres, incluindo letras, números e caracteres especiais." });

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new Users
            {
                Username = dto.Username,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Password = hashedPassword,
                Admin = false
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            await _publishEndpoint.Publish(new UserCreatedEvent
            {
                UserId = user.Id, 
                Username = user.Username, 
                Email = user.Email
            });

            return Ok(new { mensagem = "Usuário cadastrado com sucesso!" });
        }

    }
}
