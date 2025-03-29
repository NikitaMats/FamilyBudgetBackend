using FamilyBudgetBackend.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FamilyBudgetBackend.Model;
using FamilyBudgetBackend.DTOs;

namespace FamilyBudgetBackend.Controllers
{
    [ApiController]                         // Говорит, что это API-контроллер
    [Route("api/[controller]")]             // Базовый путь: /api/transactions
    public class TransactionsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;  // Подключение к БД

        // Конструктор (внедрение зависимости)
        public TransactionsController(ApplicationDbContext db)
        {
            _db = db;  // Получаем контекст БД из DI-контейнера
        }

        // GET /api/transactions
        [HttpGet]
        public async Task<ActionResult<List<Transaction>>> GetAll()
        {
            return await _db.Transactions
                .Include(t => t.User)      // Подгружаем связанные данные пользователя
                .Include(t => t.Category)  // Подгружаем категорию
                .ToListAsync();            // Асинхронно получаем список
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetById(int id)
        {
            var transaction = await _db.Transactions
                .Include(t => t.User)
                .Include(t => t.Category)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (transaction == null)
            {
                return NotFound();
            }

            return transaction;
        }

        // POST /api/transactions
        [HttpPost]
        public async Task<ActionResult<Transaction>> CreateTransaction(
        [FromBody] TransactionCreateDto transactionDto)
        {
            // Проверяем существование пользователя
            var user = await _db.Users
                .Include(u => u.Transactions) // Явно включаем транзакции
                .FirstOrDefaultAsync(u => u.Id == transactionDto.UserId);

            if (user == null)
                return NotFound("User not found");


            // Проверяем существование категории
            var categoryExists = await _db.Categories
                .AnyAsync(c => c.Id == transactionDto.CategoryId);

            if (!categoryExists)
                return NotFound("Category not found");

            // Создаем новую транзакцию
            var transaction = new Transaction
            {
                Amount = transactionDto.Amount,
                Date = transactionDto.Date,
                Description = transactionDto.Description,
                UserId = transactionDto.UserId,
                CategoryId = transactionDto.CategoryId
            };

            // Добавляем транзакцию пользователю
            user.Transactions.Add(transaction);

            try
            {
                await _db.SaveChangesAsync();
                return CreatedAtAction(nameof(GetById), new { id = transaction.Id }, transaction);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest($"Error saving transaction: {ex.InnerException?.Message}");
            }
        }
    }
}

