using Microsoft.AspNetCore.Mvc;
using Web_API_Assignment.Models;
using Web_API_Assignment.Services;

namespace Web_API_Assignment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorsController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Doctor>>> GetAll()
        {
            var doctors = await _doctorService.GetAll();
            return Ok(doctors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Doctor>> GetById(int id)
        {
            var doctor = await _doctorService.GetById(id);
            if (doctor == null) return NotFound();
            return Ok(doctor);
        }

        [HttpPost("{performedByUserId}")]
        public async Task<ActionResult<int>> Create([FromBody] Doctor doctor, int performedByUserId)
        {
            var newId = await _doctorService.CreateDoctor(doctor, performedByUserId);
            return Ok(newId);
        }

        [HttpPut("{performedByUserId}")]
        public async Task<ActionResult> Update([FromBody] Doctor doctor, int performedByUserId)
        {
            var updated = await _doctorService.UpdateDoctor(doctor, performedByUserId);
            if (!updated) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}/{performedByUserId}")]
        public async Task<ActionResult> Delete(int id, int performedByUserId)
        {
            var deleted = await _doctorService.DeleteDoctor(id, performedByUserId);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
