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

        /// <summary>
        /// Api для получения полного списка категорий.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<Category>>> GetAll() =>
            await _db.Categories.Include(c => c.TransactionType).ToListAsync();

        /// <summary>
        /// Api для получения категории через её ID.
        /// </summary>
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

        /// <summary>
        /// Api для создания новой категории.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory(
        [FromBody] CategoryCreateDto categoryDto)
        {
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

        /// <summary>
        /// Api для удаления категории.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _db.Categories.FindAsync(id);
            if (category == null) return NotFound();

            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Api для редактирование категории через её ID.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryCreateDto categoryDto)
        {
            var category = await _db.Categories.FindAsync(id);
            if (category == null) return NotFound();

            // Проверяем существование типа транзакции
            var typeExists = await _db.TransactionTypes
                .AnyAsync(tt => tt.Id == categoryDto.TransactionTypeId);

            if (!typeExists)
                return BadRequest("TransactionType not found");

            category.Name = categoryDto.Name;
            category.TransactionTypeId = categoryDto.TransactionTypeId;

            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
