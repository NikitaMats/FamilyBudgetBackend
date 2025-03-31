using FamilyBudgetBackend.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FamilyBudgetBackend.Model;
using FamilyBudgetBackend.DTOs;

namespace FamilyBudgetBackend.Controllers
{
    [ApiController]                         
    [Route("api/[controller]")]             
    public class TransactionsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;  

        public TransactionsController(ApplicationDbContext db)
        {
            _db = db;  
        }

        [HttpGet]
        public async Task<ActionResult<List<Transaction>>> GetAll(
        [FromQuery] string? type,
        [FromQuery] string? search,
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] int? categoryId)
        {
            var query = _db.Transactions
                .Include(t => t.User)
                .Include(t => t.Category)
                .AsQueryable();

            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(t => t.Category.TransactionType.Name ==
                    (type == "income" ? "Доход" : "Расход"));
            }

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(t => t.Description.Contains(search));
            }

            if (startDate.HasValue)
            {
                query = query.Where(t => t.Date >= startDate);
            }

            if (endDate.HasValue)
            {
                query = query.Where(t => t.Date <= endDate);
            }

            if (categoryId.HasValue)
            {
                query = query.Where(t => t.CategoryId == categoryId);
            }

            return await query.ToListAsync();
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

        [HttpPost]
        public async Task<ActionResult<Transaction>> CreateTransaction(
        [FromBody] TransactionCreateDto transactionDto)
        {
            var user = await _db.Users
                .Include(u => u.Transactions) 
                .FirstOrDefaultAsync(u => u.Id == transactionDto.UserId);

            if (user == null)
                return NotFound("User not found");

            var categoryExists = await _db.Categories
                .AnyAsync(c => c.Id == transactionDto.CategoryId);

            if (!categoryExists)
                return NotFound("Category not found");

            var transaction = new Transaction
            {
                Amount = transactionDto.Amount,
                Date = transactionDto.Date,
                Description = transactionDto.Description,
                UserId = transactionDto.UserId,
                CategoryId = transactionDto.CategoryId
            };

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

        [HttpGet("balance")]
        public async Task<ActionResult<decimal>> GetBalance()
        {
            var income = await _db.Transactions
                .Where(t => t.Category.TransactionType.Name == "Доход")
                .SumAsync(t => t.Amount);

            var expense = await _db.Transactions
                .Where(t => t.Category.TransactionType.Name == "Расход")
                .SumAsync(t => t.Amount);

            return Ok(income - expense);
        }

        [HttpGet("by-categories")]
        public async Task<ActionResult<List<CategoryReportDto>>> GetByCategories(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate)
        {
            var query = _db.Transactions.AsQueryable();

            if (startDate.HasValue)
                query = query.Where(t => t.Date >= startDate);

            if (endDate.HasValue)
                query = query.Where(t => t.Date <= endDate);

            var report = await query
                .GroupBy(t => new {
                    CategoryName = t.Category.Name,
                    TransactionTypeName = t.Category.TransactionType.Name
                })
                .Select(g => new CategoryReportDto
                {
                    Category = g.Key.CategoryName,
                    Type = g.Key.TransactionTypeName,
                    Total = g.Sum(t => t.Amount),
                    Percentage = 0
                })
                .ToListAsync();

            var total = report.Sum(r => r.Total);
            foreach (var item in report)
            {
                item.Percentage = total != 0 ? (item.Total / total) * 100 : 0;
            }

            return Ok(report);
        }

        [HttpGet("by-users")]
        public async Task<ActionResult<List<UserReportDto>>> GetByUsers()
        {
            return await _db.Users
                .Select(u => new UserReportDto
                {
                    UserId = u.Id,
                    UserName = u.Name,
                    TotalIncome = u.Transactions
                        .Where(t => t.Category.TransactionType.Name == "Доход")
                        .Sum(t => t.Amount),
                    TotalExpense = u.Transactions
                        .Where(t => t.Category.TransactionType.Name == "Расход")
                        .Sum(t => t.Amount)
                })
                .ToListAsync();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransaction(int id, [FromBody] TransactionUpdateDto dto)
        {
            var transaction = await _db.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            transaction.Amount = dto.Amount;
            transaction.Date = dto.Date;
            transaction.Description = dto.Description;
            transaction.CategoryId = dto.CategoryId;

            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}

