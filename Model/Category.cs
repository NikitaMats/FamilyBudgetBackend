using System.Text.Json.Serialization;

namespace FamilyBudgetBackend.Model
{
    public class Category
    {
        public int Id { get; set; }                  // ID категории
        public string Name { get; set; }             // Название ("Еда", "Транспорт")

        // Связь с типом транзакции (доход/расход)
        public int TransactionTypeId { get; set; }

        [JsonIgnore]
        public TransactionType TransactionType { get; set; }
    }
}
