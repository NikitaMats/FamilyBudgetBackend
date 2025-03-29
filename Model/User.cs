namespace FamilyBudgetBackend.Model
{
    public class User
    {
        public int Id { get; set; }                  // ID пользователя
        public string Name { get; set; }             // Имя ("Иван Иванов")
        public string Email { get; set; }            // Email ("ivan@example.com")

        // Навигационное свойство (1 пользователь → N транзакций)
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
