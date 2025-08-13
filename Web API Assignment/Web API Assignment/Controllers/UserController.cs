using Microsoft.AspNetCore.Mvc;
using Web_API_Assignment.Models;
using Web_API_Assignment.Services;

namespace Web_API_Assignment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            var users = await _userService.GetAll();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(int id)
        {
            var user = await _userService.GetById(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost("{performedByUserId}")]
        public async Task<ActionResult<int>> Create([FromBody] User user, int performedByUserId)
        {
            var newId = await _userService.CreateUser(user, performedByUserId);
            return Ok(newId);
        }

        [HttpPut("{performedByUserId}")]
        public async Task<ActionResult> Update([FromBody] User user, int performedByUserId)
        {
            var updated = await _userService.UpdateUser(user, performedByUserId);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}/{performedByUserId}")]
        public async Task<ActionResult> Delete(int id, int performedByUserId)
        {
            var deleted = await _userService.DeleteUser(id, performedByUserId);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
