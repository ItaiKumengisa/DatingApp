using API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    
    public class UsersController : BaseApiController
    {
        private readonly DataContext _dbContext;
        public UsersController(DataContext dbContext)
        {
            _dbContext = dbContext;
        }   

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _dbContext.Users.ToListAsync();

            return StatusCode(StatusCodes.Status200OK, users);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);

            return StatusCode(StatusCodes.Status200OK, user);
        }

        
    }
}
