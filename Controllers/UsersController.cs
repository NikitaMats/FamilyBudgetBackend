using FamilyBudgetBackend.Data;
using FamilyBudgetBackend.DTOs;
using FamilyBudgetBackend.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FamilyBudgetBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public UsersController(ApplicationDbContext db) => _db = db;

        /// <summary>
        /// Api for getting list of users.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAll() => await _db.Users.ToListAsync();

        /// <summary>
        /// Api for creating a user.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<User>> Create([FromBody] User user)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return Ok(user);
        }

        /// <summary>
        /// Api for getting user by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDetailDto>> GetById(
        int id,
        [FromQuery] bool includeTransactions = false)
        {
            var query = _db.Users.AsQueryable();

            if (includeTransactions)
            {
                query = query.Include(u => u.Transactions);
            }

            var user = await query.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            var result = new UserDetailDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Transactions = includeTransactions
                    ? user.Transactions.Select(t => new TransactionDto
                    {
                        Id = t.Id,
                        Amount = t.Amount,
                        Date = t.Date,
                        Description = t.Description
                    }).ToList()
                    : null
            };

            return Ok(result);
        }

        /// <summary>
        /// Api for editing user by ID.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserUpdateDto dto)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null) return NotFound();

            user.Name = dto.Name;
            user.Email = dto.Email;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Api for deleting user by ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user == null) return NotFound();

            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }

}
