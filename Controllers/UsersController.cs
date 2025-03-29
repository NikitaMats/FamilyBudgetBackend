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

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAll() => await _db.Users.ToListAsync();

        [HttpPost]
        public async Task<ActionResult<User>> Create([FromBody] User user)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return Ok(user);
        }

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
    }

}
