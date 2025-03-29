using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FamilyBudgetBackend.DTOs
{
    public class TransactionCreateDto
    {
        [Required] public decimal Amount { get; set; }
        [JsonIgnore] public DateTime Date { get; set; } = DateTime.UtcNow;
        public string Description { get; set; }
        [Required] public int UserId { get; set; }
        [Required] public int CategoryId { get; set; }
    }
}
