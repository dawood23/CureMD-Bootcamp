using careliteBackend.DTOs;
using careliteBackend.Models;
using careliteBackend.Services.BillService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Stripe;

namespace careliteBackend.Controllers
{
    [ApiController]
    [Route("billing")]
    public class BillingController : ControllerBase
    {
        private readonly IBillService _billService;

        public BillingController(IBillService billService)
        {
            _billService = billService;
        }

        [HttpPost("generate/{appointmentId}")]
        public async Task<IActionResult> GenerateBill(int appointmentId)
        {
            try
            {
                var bill = await _billService.GenerateBill(appointmentId);

                if (bill == null)
                    return NotFound(new { message = "Bill could not be generated." });

                return Ok(bill);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBills()
        {

            try
            {
                var bill = await _billService.GenerateBillList();

                if (bill == null)
                    return NotFound(new { message = "Bill list not found" });

                return Ok(bill);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

        [HttpPost("record")]
        public async Task<IActionResult> RecordPayment([FromBody] PaymentRequest request)
        {
            if (request == null)
                return BadRequest(new { Success = false, Message = "Invalid request payload." });

            try
            {
                var result = await _billService.RecordPayment(request);
                return Ok(new { Success = true, Data = result });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Success = false, Message = ex.Message });
            }
            catch (SqlException ex)
            {
                return Conflict(new { Success = false, Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = false, Message = ex.Message });
            }
        }

        [HttpGet("payments")]
        public async Task<IActionResult> GetPayments()
        {
            try
            {
                var payments = await _billService.GetPayments();
                return Ok(payments);
            }
            catch (SqlException ex)
            {
                return StatusCode(500, new
                {
                    Message = "A database error occurred while fetching payments.",
                    Details = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An unexpected error occurred while fetching payments.",
                    Details = ex.Message
                });
            }
        }
    }

}


