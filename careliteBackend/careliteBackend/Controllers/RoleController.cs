using careliteBackend.Models;
using careliteBackend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace careliteBackend.Controllers
{
    [ApiController]
    [Route("roles")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [EnableRateLimiting("SensitiveActions")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateRole([FromBody] Role role)
        {
            var id = await _roleService.CreateRole(role);
            return Ok(new { RoleID = id, Message = "Role created successfully" });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            var role = await _roleService.GetRoleById(id);
            return role == null ? NotFound() : Ok(role);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles() => Ok(await _roleService.GetAllRoles());

        [HttpPut("update")]
        public async Task<IActionResult> UpdateRole([FromBody] Role role)
        {
            var rows = await _roleService.UpdateRole(role);
            return Ok(new { RowsAffected = rows, Message = "Role updated successfully" });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var rows = await _roleService.DeleteRole(id);
            return Ok(new { RowsAffected = rows, Message = "Role deleted successfully" });
        }
    }
}
