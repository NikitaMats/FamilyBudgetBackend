namespace FamilyBudgetBackend.Model
{
    public class TransactionType
    {
        private int _id;
        private string _name;

        public int Id
        {
            get => _id;
            set => _id = value > 0 ? value
                : throw new ArgumentException("Id must be positive");
        }

        public string Name
        {
            get => _name;
            set => _name = !string.IsNullOrWhiteSpace(value) ? value
                : throw new ArgumentNullException(nameof(Name));
        }

        // Навигационное свойство для связи с категориями
        public ICollection<Category> Categories { get; set; } = new List<Category>();
    }
}
