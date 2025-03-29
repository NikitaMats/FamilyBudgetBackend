namespace FamilyBudgetBackend.DTOs
{
    public class UserDetailDto : UserDto
    {
        public List<TransactionDto> Transactions { get; set; }
    }
}
