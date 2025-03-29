using System.ComponentModel.DataAnnotations;

namespace FamilyBudgetBackend.DTOs
{
    public class UserCreateDto
    {
        [Required] public string Name { get; set; }
        [Required][EmailAddress] public string Email { get; set; }
    }
}
