using Microsoft.AspNetCore.Mvc;
using Web_API_Assignment.Models;
using Web_API_Assignment.Services;

namespace Web_API_Assignment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VisitTypesController : ControllerBase
    {
        private readonly IVisitTypeService _visitTypeService;

        public VisitTypesController(IVisitTypeService visitTypeService)
        {
            _visitTypeService = visitTypeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VisitType>>> GetAll()
        {
            var visitTypes = await _visitTypeService.GetAll();
            return Ok(visitTypes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VisitType>> GetById(int id)
        {
            var visitType = await _visitTypeService.GetById(id);
            if (visitType == null) return NotFound();
            return Ok(visitType);
        }

        [HttpPost("{performedByUserId}")]
        public async Task<ActionResult<int>> Create([FromBody] VisitType visitType, int performedByUserId)
        {
            var newId = await _visitTypeService.CreateVisitType(visitType, performedByUserId);

            return Ok(newId);
        }

        [HttpPut("{performedByUserId}")]
        public async Task<ActionResult> Update([FromBody] VisitType visitType, int performedByUserId)
        {
            var updated = await _visitTypeService.UpdateVisitType(visitType, performedByUserId);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}/{performedByUserId}")]
        public async Task<ActionResult> Delete(int id, int performedByUserId)
        {
            var deleted = await _visitTypeService.DeleteVisitType(id, performedByUserId);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
