using System.ComponentModel.DataAnnotations.Schema;

namespace FCG.Domain.Entities
{
    [Table("Users", Schema = "users")]
    public class Users
    {
        public int Id { get; set; }
        public required string Username { get; set; } = string.Empty;
        public required string FirstName { get; set; } = string.Empty;
        public required string LastName { get; set; } = string.Empty;
        public required string Email { get; set; } = string.Empty;
        public required bool Admin { get; set; } = false;
        public required string Password { get; set; } = string.Empty;
    }
}
