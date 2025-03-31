namespace FamilyBudgetBackend.DTOs
{
    public class UserReportDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal Balance => TotalIncome - TotalExpense;
    }
}
