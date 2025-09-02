using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web_API_Assignment.Models;
using Web_API_Assignment.Services;

namespace Web_API_Assignment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Patient>>> GetAll()
        {
            var patients = await _patientService.GetAll();
            return Ok(patients);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetById(int id)
        {
            var patient = await _patientService.GetById(id);
            if (patient == null) return NotFound();
            return Ok(patient);
        }

        [HttpPost("{performedByUserId}")]
        public async Task<ActionResult<int>> Create([FromBody] Patient patient, int performedByUserId)
        {
            var newId = await _patientService.CreatePatient(patient, performedByUserId);
            return Ok(newId);
        }

        [HttpPut("{performedByUserId}")]
        public async Task<ActionResult> Update([FromBody] Patient patient, int performedByUserId)
        {
            var updated = await _patientService.UpdatePatient(patient, performedByUserId);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}/{performedByUserId}")]
        public async Task<ActionResult> Delete(int id, int performedByUserId)
        {
            var deleted = await _patientService.DeletePatient(id, performedByUserId);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
