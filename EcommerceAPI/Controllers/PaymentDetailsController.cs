using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcommerceAPI.Models;

namespace EcommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentDetailsController : ControllerBase
    {
        private readonly EcommerceContext _context;
        private readonly IPayPalService _payPalService;

        public PaymentDetailsController(EcommerceContext context, IPayPalService payPalService)
        {
            _context = context;
            _payPalService = payPalService;
        }

        // GET: api/PaymentDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentDetail>>> GetPaymentdetails()
        {
          if (_context.Paymentdetails == null)
          {
              return NotFound();
          }
            return await _context.Paymentdetails.ToListAsync();
        }

        // GET: api/PaymentDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentDetail>> GetPaymentDetail(int id)
        {
          if (_context.Paymentdetails == null)
          {
              return NotFound();
          }
            var paymentDetail = await _context.Paymentdetails.FindAsync(id);

            if (paymentDetail == null)
            {
                return NotFound();
            }

            return paymentDetail;
        }

        // PUT: api/PaymentDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaymentDetail(int id, PaymentDetail paymentDetail)
        {
            if (id != paymentDetail.PaymentId)
            {
                return BadRequest();
            }

            _context.Entry(paymentDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentDetailExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/PaymentDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PaymentDetail>> PostPaymentDetail(PaymentDetail paymentDetail)
        {
          if (_context.Paymentdetails == null)
          {
              return Problem("Entity set 'EcommerceContext.Paymentdetails'  is null.");
          }
            _context.Paymentdetails.Add(paymentDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPaymentDetail", new { id = paymentDetail.PaymentId }, paymentDetail);
        }

        // DELETE: api/PaymentDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaymentDetail(int id)
        {
            if (_context.Paymentdetails == null)
            {
                return NotFound();
            }
            var paymentDetail = await _context.Paymentdetails.FindAsync(id);
            if (paymentDetail == null)
            {
                return NotFound();
            }

            _context.Paymentdetails.Remove(paymentDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/PaymentDetails/CreatePayPalPayment
        [HttpPost("CreatePayPalPayment")]
        public async Task<ActionResult<string>> CreatePayPalPayment(decimal amount, string currency, string returnUrl, string cancelUrl)
        {
            try
            {
                // Create a payment using the PayPal service
                var approvalUrl = await _payPalService.CreatePayment(amount, currency, returnUrl, cancelUrl);

                if (!string.IsNullOrEmpty(approvalUrl))
                {
                    return Ok(new { ApprovalUrl = approvalUrl });
                }
                else
                {
                    return BadRequest("Unable to create PayPal payment.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception (consider using a logging framework)
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating payment: {ex.Message}");
            }
        }

        // POST: api/PaymentDetails/ExecutePayPalPayment
        [HttpPost("ExecutePayPalPayment")]
        public async Task<IActionResult> ExecutePayPalPayment(string paymentId, string payerId)
        {
            try
            {
                // Execute the payment using the PayPal service
                var success = await _payPalService.ExecutePayment(paymentId, payerId);
                if (success)
                {
                    // Update your database as needed
                    return Ok("Payment executed successfully.");
                }
                else
                {
                    return BadRequest("Payment execution failed.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception (consider using a logging framework)
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error executing payment: {ex.Message}");
            }
        }
        private bool PaymentDetailExists(int id)
        {
            return (_context.Paymentdetails?.Any(e => e.PaymentId == id)).GetValueOrDefault();
        }
    }
}
