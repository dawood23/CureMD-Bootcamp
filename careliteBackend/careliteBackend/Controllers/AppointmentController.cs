using careliteBackend.DTOs;
using careliteBackend.Models;
using careliteBackend.Services.AppointmentService;
using careliteBackend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace careliteBackend.Controllers
{
    [ApiController]
    [Route("appointments")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _service;

        public AppointmentController(IAppointmentService service)
        {
            _service = service;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAppointment([FromBody] AppointmentRequest request)
        {
            if (request == null)
                return BadRequest(new { Success = false, Message = "Invalid request payload." });

            try
            {
                var id = await _service.Create(request);

                if (id <= 0)
                    return Conflict(new { Success = false, Message = "Unable to create appointment." });

                return Ok(new { Success = true, AppointmentID = id });
            }
            catch (SqlException ex)
            {
                return Conflict(new { Success = false, Message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAppointments()
        {
            var result = await _service.GetAll();

            if (result == null || !result.Any())
                return NotFound(new { Success = false, Message = "No appointments found." });

            return Ok(new { Success = true, Data = result });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAppointment([FromBody] AppointmentRequest request)
        {
            if (request == null)
                return BadRequest(new { Success = false, Message = "Invalid request payload." });

            try
            {
                var rows = await _service.Update(request);

                if (rows == 0)
                    return NotFound(new { Success = false, Message = $"Appointment with ID not found." });

                return Ok(new { Success = true, UpdatedRows = rows });
            }
            catch (SqlException ex)
            {
                return Conflict(new { Success = false, Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var rows = await _service.Delete(id);

            if (rows == 0)
                return NotFound(new { Success = false, Message = $"Appointment with ID {id} not found." });

            return Ok(new { Success = true, DeletedRows = rows });
        }

        [HttpGet("weekly-calendar/{doctorId}/{weekStartDate}")]
        public async Task<ActionResult<IEnumerable<object>>> GetWeeklyCalendar(int doctorId, string weekStartDate)
        {
            try
            {
                var date = DateTime.Parse(weekStartDate);
                var appointments = await _service.GetWeeklyCalendar(doctorId, date);
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
