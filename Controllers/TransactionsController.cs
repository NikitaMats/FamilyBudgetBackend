using FamilyBudgetBackend.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FamilyBudgetBackend.Model;

namespace FamilyBudgetBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TransactionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
        {
            return await _context.Transactions.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Transaction>> AddTransaction(Transaction transaction)
        {
            // Установите текущую дату, если она не указана
            transaction.Date = transaction.Date == default ? DateTime.UtcNow : transaction.Date;

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            // Возвращаем 201 Created с указанием URI нового ресурса
            return CreatedAtAction(
                nameof(GetTransaction), // Используем метод GetTransaction (см. шаг 2)
                new { id = transaction.Id },
                transaction
            );
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransaction(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);

            if (transaction == null)
            {
                return NotFound();
            }

            return transaction;
        }
    }
}
