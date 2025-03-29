using System.ComponentModel.DataAnnotations;

namespace FamilyBudgetBackend.DTOs
{
    public class TransactionTypeCreateDto
    {
        [Required] public string Name { get; set; }
    }
}
