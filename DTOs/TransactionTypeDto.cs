using System.ComponentModel.DataAnnotations;

namespace FamilyBudgetBackend.DTOs
{
    public class TransactionTypeDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название типа обязательно")]
        [StringLength(50, ErrorMessage = "Название не должно превышать 50 символов")]
        public string Name { get; set; }
    }
}
