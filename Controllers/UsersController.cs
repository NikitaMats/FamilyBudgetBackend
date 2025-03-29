using FamilyBudgetBackend.Data;
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
    }

}
