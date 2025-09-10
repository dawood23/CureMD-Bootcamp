using careliteBackend.DTOs;
using careliteBackend.Services.VisitService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace VisitManager.API.Controllers
{
    [ApiController]
    [Route("VisitNote")]
    public class VisitsController : ControllerBase
    {
        private readonly IVisitService _service;

        public VisitsController(IVisitService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetVisits()
        {
            var visits = await _service.GetAllVisits();

            if (visits == null || !visits.Any())
                return NotFound(new { Success = false, Message = "No visits found." });

            return Ok(new { Success = true, Data = visits });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVisitById(int id)
        {
            if (id <= 0)
                return BadRequest(new { Success = false, Message = "Invalid visit ID." });

            var result = await _service.GetVisitById(id);

            if (result == null)
                return NotFound(new { Success = false, Message = $"Visit with AppointmentID {id} not found." });

            return Ok(new { Success = true, Data = result });
        }

        [HttpPost]
        public async Task<IActionResult> CreateVisit([FromBody] CreateVisitRequest request)
        {
            if (request == null)
                return BadRequest(new { Success = false, Message = "Invalid request payload." });

            try
            {
                var id = await _service.CreateVisit(request);

                if (id <= 0)
                    return Conflict(new { Success = false, Message = "Unable to create visit." });

                return Ok(new { Success = true, VisitNoteID = id });
            }
            catch (SqlException ex)
            {
                return Conflict(new { Success = false, Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVisit(int id, [FromBody] UpdateVisitRequest request)
        {
            Console.WriteLine("Content in controller: ", request.content);
            if (id <= 0 || request == null)
                return BadRequest(new { Success = false, Message = "Invalid request." });

            try
            {
                var rows = await _service.UpdateVisit(id, request);

                if (rows == 0)
                    return NotFound(new { Success = false, Message = $"Visit with ID {id} not found." });

                return Ok(new { Success = true, UpdatedRows = rows });
            }
            catch (SqlException ex)
            {
                return Conflict(new { Success = false, Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVisit(int id)
        {
            if (id <= 0)
                return BadRequest(new { Success = false, Message = "Invalid visit ID." });

            var rows = await _service.DeleteVisit(id);

            if (rows == 0)
                return NotFound(new { Success = false, Message = $"Visit with ID {id} not found." });

            return Ok(new { Success = true, DeletedRows = rows });
        }
    }
}
