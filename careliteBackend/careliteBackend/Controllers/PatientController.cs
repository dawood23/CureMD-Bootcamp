using careliteBackend.Models;
using careliteBackend.Services.PatientService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace careliteBackend.Controllers
{
    [ApiController]
    [Route("patients")]
    [Authorize(Policy = "RequireStaffOrAdmin")]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _service;
        public PatientsController(IPatientService service) => _service = service;

        [EnableRateLimiting("SensitiveActions")]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] Patient patient)
        {
            if (patient == null || string.IsNullOrWhiteSpace(patient.FirstName) || string.IsNullOrWhiteSpace(patient.LastName))
                return BadRequest(new { Message = "Invalid patient data" });

            try
            {
                var id = await _service.CreatePatient(patient);
                if (id > 0)
                    return CreatedAtAction(nameof(Get), new { id }, new { PatientID = id, Message = "Patient created successfully" });

                return StatusCode(500, new { Message = "Failed to create patient" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error occurred while creating patient", Details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            if (id <= 0) return BadRequest(new { Message = "Invalid patient ID" });

            try
            {
                var patient = await _service.GetPatientById(id);
                return patient == null ? NotFound(new { Message = "Patient not found" }) : Ok(patient);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error occurred while fetching patient", Details = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var patients = await _service.GetAllPatients();
                if (patients == null || !patients.Any())
                    return NotFound(new { Message = "No patients found" });

                return Ok(patients);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error occurred while fetching patients", Details = ex.Message });
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] Patient patient)
        {
            if (patient == null || patient.PatientID <= 0)
                return BadRequest(new { Message = "Invalid patient data" });

            try
            {
                var rows = await _service.UpdatePatient(patient);
                if (rows > 0)
                    return Ok(new { RowsAffected = rows, Message = "Patient updated successfully" });

                return NotFound(new { Message = "Patient not found or no changes made" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error occurred while updating patient", Details = ex.Message });
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest(new { Message = "Invalid patient ID" });

            try
            {
                var rows = await _service.DeletePatient(id);
                if (rows > 0)
                    return Ok(new { RowsAffected = rows, Message = "Patient deleted successfully" });

                return NotFound(new { Message = "Patient not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error occurred while deleting patient", Details = ex.Message });
            }
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged(
             [FromQuery] int pageNumber = 1,
             [FromQuery] int pageSize = 10,
             [FromQuery] string search = "")
        {
            try
            {
                var (patients, totalCount) = await _service.GetPatientsPaged(pageNumber, pageSize, search);

                return Ok(new
                {
                    Data = patients,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error occurred while fetching paginated patients", Details = ex.Message });
            }
        }


    }
}
