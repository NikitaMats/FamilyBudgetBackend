using System.Text.Json.Serialization;

namespace FamilyBudgetBackend.Model
{
    public class TransactionType
    {
        public int Id { get; set; }
        public string Name { get; set; }  

        [JsonIgnore]
        public List<Category> Categories { get; set; } = new();
    }
}
