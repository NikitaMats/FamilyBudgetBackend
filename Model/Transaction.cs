using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FamilyBudgetBackend.Model
{
    public class Transaction
    {
        public int Id { get; set; }                  // Уникальный ID транзакции (автоинкремент)
        public decimal Amount { get; set; }          // Сумма (например, 1500.50)
        public DateTime Date { get; set; }           // Дата (2023-11-20T14:30:00Z)
        public string Description { get; set; }      // Описание ("Продукты в Пятерочке")

        // Внешние ключи
        public int UserId { get; set; }              // ID пользователя
        public int CategoryId { get; set; }          // ID категории

        // Навигационные свойства (связи между таблицами)
        [JsonIgnore]
        public User User { get; set; }               // Какой пользователь создал транзакцию
        [JsonIgnore]
        public Category Category { get; set; }       // Какая категория (еда, транспорт)
    }
}
