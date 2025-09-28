using Xunit;
using FCG.Controllers;
using FCG.Application.DTOs;
using FCG.Domain.Entities;
using FCG.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;

namespace FCG.Tests.Domain
{
    public class AccountControllerTests
    {
        private readonly DataContext _context;
        private readonly AccountController _controller;

        public AccountControllerTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // garante banco isolado
                .Options;

            _context = new DataContext(options);
            _controller = new AccountController(_context);
        }

        [Fact]
        public async Task Register_Register_Valid()
        {
            var dto = new AccountDto
            {
                Username = "usuario_teste",
                FirstName = "Nome",
                LastName = "Sobrenome",
                Email = "teste@email.com",
                Password = "Senha@25"
            };

            var result = await _controller.Register(dto);

            result.Should().BeOfType<OkObjectResult>();
            var users = _context.Users.ToList();
            users.Should().ContainSingle();
            users[0].Username.Should().Be("usuario_teste");
        }

        [Fact]
        public async Task Register_ReturnError_EmailAlreadyExists()
        {
            _context.Users.Add(new Users
            {
                Username = "usuario_teste",
                FirstName = "Nome",
                LastName = "Sobrenome",
                Email = "teste@email.com",
                Password = "Senha@25",
                Admin = false
            });
            await _context.SaveChangesAsync();

            var dto = new AccountDto
            {
                Username = "outrousuario_teste",
                FirstName = "OutroNome",
                LastName = "OutroSobrenome",
                Email = "teste@email.com",
                Password = "Senha@25"
            };

            var result = await _controller.Register(dto);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Register_ReturnError_InvalidEmail()
        {
            var dto = new AccountDto
            {
                Username = "usuario_teste",
                FirstName = "Nome",
                LastName = "Sobrenome",
                Email = "teste@email",
                Password = "Senha@25"
            };

            var result = await _controller.Register(dto);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Register_ReturnError_PasswordWeak()
        {
            var dto = new AccountDto
            {
                Username = "usuario_teste",
                FirstName = "Nome",
                LastName = "Sobrenome",
                Email = "teste@email.com",
                Password = "Senha@"
            };

            var result = await _controller.Register(dto);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Register_ReturnError_UsernameAlreadyExists()
        {
            _context.Users.Add(new Users
            {
                Username = "usuario_teste",
                FirstName = "Nome",
                LastName = "Sobrenome",
                Email = "teste@email.com",
                Password = "Senha@25",
                Admin = false
            });
            await _context.SaveChangesAsync();

            var dto = new AccountDto
            {
                Username = "usuario_teste",
                FirstName = "OutroNome",
                LastName = "OutroSobrenome",
                Email = "outroteste@email.com",
                Password = "Senha@25"
            };

            var result = await _controller.Register(dto);

            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}