using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web_API_Assignment.Models;
using Web_API_Assignment.Services;

namespace Web_API_Assignment.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserRolesController : ControllerBase
    {
        private readonly IUserRoleService _userRoleService;

        public UserRolesController(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserRole>>> GetAll()
        {
            var roles = await _userRoleService.GetAll();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserRole>> GetById(int id)
        {
            var role = await _userRoleService.GetById(id);
            if (role == null) return NotFound();
            return Ok(role);
        }

        [HttpPost("{performedByUserId}")]
        public async Task<ActionResult<int>> Create([FromBody] UserRole role, int performedByUserId)
        {
            var newId = await _userRoleService.CreateUserRole(role, performedByUserId);
            return Ok(newId);
        }

        [HttpPut("{performedByUserId}")]
        public async Task<ActionResult> Update([FromBody] UserRole role, int performedByUserId)
        {
            var updated = await _userRoleService.UpdateUserRole(role, performedByUserId);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}/{performedByUserId}")]
        public async Task<ActionResult> Delete(int id, int performedByUserId)
        {
            var deleted = await _userRoleService.DeleteUserRole(id, performedByUserId);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
