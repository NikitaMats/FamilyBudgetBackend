using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FamilyBudgetBackend.Model
{
    public class Transaction
    {
        public int Id { get; set; }                  
        public decimal Amount { get; set; }          
        public DateTime Date { get; set; }          
        public string Description { get; set; }     

        public int UserId { get; set; }              
        public int CategoryId { get; set; }         

        [JsonIgnore]
        public User User { get; set; }               
        [JsonIgnore]
        public Category Category { get; set; }       
    }
}
