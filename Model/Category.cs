namespace FamilyBudgetBackend.Model
{
    public class Category
    {
        private int _id;
        private string _name;
        private int _transactionTypeId;

        public int Id { get; set; }
        public string Name
        {
            get => _name;
            set => _name = value ?? throw new ArgumentNullException(nameof(value));
        }

        public int TransactionTypeId
        {
            get => _transactionTypeId;
            set => _transactionTypeId = value > 0 ? value
                : throw new ArgumentException("TransactionTypeId must be positive");
        }

        // Навигационное свойство
        public TransactionType TransactionType { get; set; }
    }
}
