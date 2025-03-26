namespace FamilyBudgetBackend.Model
{
    public class Transaction
    {
        private int _id;
        private float _amount;
        private DateTime _date;
        private string _description;
        private int _userId;
        private int _categoryId;

        public int Id
        {
            get => _id;
            set => _id = value > 0 ? value
                : throw new ArgumentException("Id must be positive");
        }

        public float Amount
        {
            get => _amount;
            set => _amount = value != 0 ? value
                : throw new ArgumentException("Amount cannot be zero");
        }

        public DateTime Date
        {
            get => _date;
            set => _date = value <= DateTime.Now ? value
                : throw new ArgumentException("Date cannot be in the future");
        }

        public string Description
        {
            get => _description;
            set => _description = !string.IsNullOrWhiteSpace(value) ? value
                : throw new ArgumentNullException(nameof(Description));
        }

        public int UserId
        {
            get => _userId;
            set => _userId = value > 0 ? value
                : throw new ArgumentException("UserId must be positive");
        }

        public int CategoryId
        {
            get => _categoryId;
            set => _categoryId = value > 0 ? value
                : throw new ArgumentException("CategoryId must be positive");
        }

        // Навигационные свойства
        public User User { get; set; }
        public Category Category { get; set; }
    }
}
