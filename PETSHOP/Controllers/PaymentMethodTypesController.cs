using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PETSHOP.Models;

namespace PETSHOP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodTypesController : ControllerBase
    {
        private readonly PETSHOPContext _context;

        public PaymentMethodTypesController(PETSHOPContext context)
        {
            _context = context;
        }

        // GET: api/PaymentMethodTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentMethodType>>> GetPaymentMethodType()
        {
            return await _context.PaymentMethodType.ToListAsync();
        }

        // GET: api/PaymentMethodTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentMethodType>> GetPaymentMethodType(int id)
        {
            var paymentMethodType = await _context.PaymentMethodType.FindAsync(id);

            if (paymentMethodType == null)
            {
                return NotFound();
            }

            return paymentMethodType;
        }

        // PUT: api/PaymentMethodTypes/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaymentMethodType(int id, PaymentMethodType paymentMethodType)
        {
            if (id != paymentMethodType.PaymentMethodTypeId)
            {
                return BadRequest();
            }

            _context.Entry(paymentMethodType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentMethodTypeExists(id))
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

        // POST: api/PaymentMethodTypes
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<PaymentMethodType>> PostPaymentMethodType(PaymentMethodType paymentMethodType)
        {
            _context.PaymentMethodType.Add(paymentMethodType);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PaymentMethodTypeExists(paymentMethodType.PaymentMethodTypeId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPaymentMethodType", new { id = paymentMethodType.PaymentMethodTypeId }, paymentMethodType);
        }

        // DELETE: api/PaymentMethodTypes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PaymentMethodType>> DeletePaymentMethodType(int id)
        {
            var paymentMethodType = await _context.PaymentMethodType.FindAsync(id);
            if (paymentMethodType == null)
            {
                return NotFound();
            }

            _context.PaymentMethodType.Remove(paymentMethodType);
            await _context.SaveChangesAsync();

            return paymentMethodType;
        }

        private bool PaymentMethodTypeExists(int id)
        {
            return _context.PaymentMethodType.Any(e => e.PaymentMethodTypeId == id);
        }
    }
}
