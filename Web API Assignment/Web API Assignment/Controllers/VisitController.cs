using Microsoft.AspNetCore.Mvc;
using Web_API_Assignment.Models;
using Web_API_Assignment.Services;

namespace Web_API_Assignment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VisitsController : ControllerBase
    {
        private readonly IVisitService _visitService;

        public VisitsController(IVisitService visitService)
        {
            _visitService = visitService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Visit>>> GetAll()
        {
            var visits = await _visitService.GetAll();
            return Ok(visits);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Visit>> GetById(int id)
        {
            var visit = await _visitService.GetById(id);
            if (visit == null) return NotFound();
            return Ok(visit);
        }

        [HttpPost("{performedByUserId}")]
        public async Task<ActionResult<int>> Create([FromBody] Visit visit, int performedByUserId)
        {
            var newId = await _visitService.CreateVisit(visit, performedByUserId);
            return Ok(newId);
        }

        [HttpPut("{performedByUserId}")]
        public async Task<ActionResult> Update([FromBody] Visit visit, int performedByUserId)
        {
            var updated = await _visitService.UpdateVisit(visit, performedByUserId);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}/{performedByUserId}")]
        public async Task<ActionResult> Delete(int id, int performedByUserId)
        {
            var deleted = await _visitService.DeleteVisit(id, performedByUserId);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
