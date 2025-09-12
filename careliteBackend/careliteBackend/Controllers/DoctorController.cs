using careliteBackend.Models;
using careliteBackend.Services.DoctorService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace careliteBackend.Controllers
{
    [ApiController]
    [Route("doctors")]
    [Authorize]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService _service;
        public DoctorsController(IDoctorService service) => _service = service;

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] Doctor doctor)
        {
            if (doctor == null || string.IsNullOrWhiteSpace(doctor.DoctorName))
                return BadRequest(new { Message = "Invalid doctor data" });

            try
            {
                var id = await _service.CreateDoctor(doctor);
                if (id > 0)
                    return CreatedAtAction(nameof(Get), new { id }, new { DoctorID = id, Message = "Doctor created successfully" });

                return StatusCode(500, new { Message = "Failed to create doctor" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error occurred while creating doctor", Details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (id <= 0) return BadRequest(new { Message = "Invalid doctor ID" });

            try
            {
                var doctor = await _service.GetDoctorById(id);
                return doctor == null ? NotFound(new { Message = "Doctor not found" }) : Ok(doctor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error occurred while fetching doctor", Details = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var doctors = await _service.GetAllDoctors();
                if (doctors == null || !doctors.Any())
                    return NotFound(new { Message = "No doctors found" });

                return Ok(doctors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error occurred while fetching doctors", Details = ex.Message });
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] Doctor doctor)
        {
            if (doctor == null || doctor.DoctorID <= 0)
                return BadRequest(new { Message = "Invalid doctor data" });

            try
            {
                var rows = await _service.UpdateDoctor(doctor);
                if (rows > 0)
                    return Ok(new { RowsAffected = rows, Message = "Doctor updated successfully" });

                return NotFound(new { Message = "Doctor not found or no changes made" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error occurred while updating doctor", Details = ex.Message });
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest(new { Message = "Invalid doctor ID" });

            try
            {
                var rows = await _service.DeleteDoctor(id);
                if (rows > 0)
                    return Ok(new { RowsAffected = rows, Message = "Doctor deleted successfully" });

                return NotFound(new { Message = "Doctor not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error occurred while deleting doctor", Details = ex.Message });
            }
        }
    }
}
    
    
