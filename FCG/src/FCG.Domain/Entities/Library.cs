using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCG.Domain.Entities
{
    [Table("Library", Schema = "library")]
    public class Library
    {
        public int Id { get; set; }
        public required string Username { get; set; } = string.Empty;
        public int IdUser { get; set; }

        public int IdGame { get; set; }
        public DateTime PurchasedDate { get; set; }

        [Precision(18,2)]
        public decimal ValuePaid {  get; set; }
    }
}
