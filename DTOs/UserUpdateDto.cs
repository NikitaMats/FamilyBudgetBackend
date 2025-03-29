using System.ComponentModel.DataAnnotations;

namespace FamilyBudgetBackend.DTOs
{
    public class UserUpdateDto
    {
        [Required] public string Name { get; set; }
        [Required][EmailAddress] public string Email { get; set; }
    }
}
