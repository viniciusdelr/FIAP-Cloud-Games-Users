using Xunit;
using FCG.Domain.Entities;

namespace FCG.Tests.Domain
{
    public class UsersTests
    {
        private Users CreateValidUser(
            string? username = null,
            string? firstName = null,
            string? lastName = null,
            string? email = null,
            string? password = null,
            bool? admin = null)
        {
            return new Users
            {
                Username = username ?? "usuario_teste",
                FirstName = firstName ?? "Nome",
                LastName = lastName ?? "Sobrenome",
                Email = email ?? "teste@email.com",
                Password = password ?? "Senha@25",
                Admin = admin ?? false
            };
        }

        [Fact]
        public void User_Should_Have_Default_Values_Set()
        {
            var user = CreateValidUser();

            Assert.Equal("usuario_teste", user.Username);
            Assert.Equal("Nome", user.FirstName);
            Assert.Equal("Sobrenome", user.LastName);
            Assert.Equal("teste@email.com", user.Email);
            Assert.Equal("Senha@25", user.Password);
            Assert.False(user.Admin);
        }

        [Fact]
        public void Admin_User_Should_Have_Admin_True()
        {
            var user = CreateValidUser(admin: true);

            Assert.True(user.Admin);
        }

        [Fact]
        public void User_Email_Should_Contain_At_Symbol()
        {
            var user = CreateValidUser(email: "usuario@email.com");

            Assert.Contains("@", user.Email);
        }

        [Fact]
        public void User_Password_Should_Not_Be_Empty()
        {
            var user = CreateValidUser(password: "Senha@25");

            Assert.False(string.IsNullOrEmpty(user.Password));
        }

        [Fact]
        public void User_Should_Allow_Custom_Username()
        {
            var user = CreateValidUser(username: "custom_username");

            Assert.Equal("custom_username", user.Username);
        }

        [Fact]
        public void User_Should_Allow_Different_Names()
        {
            var user = CreateValidUser(firstName: "Nome", lastName: "Sobrenome");

            Assert.Equal("Nome", user.FirstName);
            Assert.Equal("Sobrenome", user.LastName);
        }
    }
}
