using System.ComponentModel.DataAnnotations;

namespace FamilyBudgetBackend.DTOs
{
    public class CategoryCreateDto
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public int TransactionTypeId { get; set; } // Только ID типа транзакции
    }
}
