namespace FamilyBudgetBackend.DTOs
{
    public class CategoryReportDto
    {
        public string Category { get; set; }
        public string Type { get; set; } // "Доход" или "Расход"
        public decimal Total { get; set; }
        public decimal Percentage { get; set; }
    }
}
