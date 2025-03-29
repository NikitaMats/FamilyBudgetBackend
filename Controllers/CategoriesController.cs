using FamilyBudgetBackend.Data;
using FamilyBudgetBackend.Model;
using FamilyBudgetBackend.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FamilyBudgetBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public CategoriesController(ApplicationDbContext db) => _db = db;

        [HttpGet]
        public async Task<ActionResult<List<Category>>> GetAll() =>
            await _db.Categories.Include(c => c.TransactionType).ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetById(int id)
        {
            var category = await _db.Categories
                .Include(c => c.TransactionType)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory(
        [FromBody] CategoryCreateDto categoryDto)
        {
            // Проверяем существование типа транзакции
            var typeExists = await _db.TransactionTypes
                .AnyAsync(tt => tt.Id == categoryDto.TransactionTypeId);

            if (!typeExists)
                return BadRequest("TransactionType not found");

            var category = new Category
            {
                Name = categoryDto.Name,
                TransactionTypeId = categoryDto.TransactionTypeId
            };

            _db.Categories.Add(category);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }
    }
}
