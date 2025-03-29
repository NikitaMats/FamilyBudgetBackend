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

        [HttpPost]
        public async Task<ActionResult<TransactionTypeDto>> Create([FromBody] TransactionTypeCreateDto dto)
        {
            var type = new TransactionType { Name = dto.Name };
            _db.TransactionTypes.Add(type);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = type.Id }, type);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var type = await _db.TransactionTypes.FindAsync(id);
            if (type == null) return NotFound();

            _db.TransactionTypes.Remove(type);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionTypeDto>> GetById(int id)
        {
            var type = await _db.TransactionTypes.FindAsync(id);
            if (type == null) return NotFound();
            return Ok(new TransactionTypeDto { Id = type.Id, Name = type.Name });
        }
    }
}
