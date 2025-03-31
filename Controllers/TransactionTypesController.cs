using FamilyBudgetBackend.Data;
using FamilyBudgetBackend.DTOs;
using FamilyBudgetBackend.Model;
using Microsoft.AspNetCore.Mvc;

namespace FamilyBudgetBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionTypesController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public TransactionTypesController(ApplicationDbContext db) => _db = db;

        /// <summary>
        /// Api for creating transaction type by ID.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<TransactionTypeDto>> Create([FromBody] TransactionTypeCreateDto dto)
        {
            var type = new TransactionType { Name = dto.Name };
            _db.TransactionTypes.Add(type);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = type.Id }, type);
        }

        /// <summary>
        /// Api for deleting transaction type by ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var type = await _db.TransactionTypes.FindAsync(id);
            if (type == null) return NotFound();

            _db.TransactionTypes.Remove(type);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Api for getting transaction type by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionTypeDto>> GetById(int id)
        {
            var type = await _db.TransactionTypes.FindAsync(id);
            if (type == null) return NotFound();
            return Ok(new TransactionTypeDto { Id = type.Id, Name = type.Name });
        }
    }
}
