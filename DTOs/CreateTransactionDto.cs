using System.Text.Json.Serialization;

namespace FamilyBudgetBackend.DTOs
{
    public class CreateTransactionDto
    {
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }

        [JsonIgnore] // Игнорируем при десериализации
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
