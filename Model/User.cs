namespace FamilyBudgetBackend.Model
{
    public class User
    {
        public int Id { get; set; }

        private string _name;
        public string Name
        {
            get => _name;
            set => _name = value ?? throw new ArgumentNullException(nameof(value));
        }

        private string _email;
        public string Email
        {
            get => _email;
            set => _email = value ?? throw new ArgumentNullException(nameof(value));
        }

        // Навигационное свойство (1 пользователь → N транзакций)
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

        // Метод для валидации email (инкапсуляция логики)
        public bool IsValidEmail()
        {
            return Email.Contains("@");
        }
    }
}
