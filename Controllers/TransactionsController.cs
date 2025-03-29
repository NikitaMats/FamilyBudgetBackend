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
        [FromBody] CreateTransactionDto transactionDto)
        {
            // Проверяем существование пользователя и категории
            var userExists = await _db.Users.AnyAsync(u => u.Id == transactionDto.UserId);
            var categoryExists = await _db.Categories.AnyAsync(c => c.Id == transactionDto.CategoryId);

            if (!userExists || !categoryExists)
            {
                return BadRequest("User or Category not found");
            }

            var transaction = new Transaction
            {
                Amount = transactionDto.Amount,
                Date = transactionDto.Date,
                Description = transactionDto.Description,
                UserId = transactionDto.UserId,
                CategoryId = transactionDto.CategoryId
            };

            _db.Transactions.Add(transaction);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = transaction.Id }, transaction);
        }
    }
}

