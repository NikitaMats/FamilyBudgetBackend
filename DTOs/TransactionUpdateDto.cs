namespace FamilyBudgetBackend.DTOs
{
    public class TransactionUpdateDto
    {
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
    }
}
