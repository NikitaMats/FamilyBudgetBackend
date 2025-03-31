using System.Text.Json.Serialization;

namespace FamilyBudgetBackend.Model
{
    public class Category
    {
        public int Id { get; set; }                
        public string Name { get; set; }             

        public int TransactionTypeId { get; set; }

        [JsonIgnore]
        public TransactionType TransactionType { get; set; }
    }
}
