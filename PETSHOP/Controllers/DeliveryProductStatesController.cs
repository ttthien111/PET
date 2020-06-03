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
    public class DeliveryProductStatesController : ControllerBase
    {
        private readonly PETSHOPContext _context;

        public DeliveryProductStatesController(PETSHOPContext context)
        {
            _context = context;
        }

        // GET: api/DeliveryProductStates
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeliveryProductState>>> GetDeliveryProductState()
        {
            return await _context.DeliveryProductState.ToListAsync();
        }

        // GET: api/DeliveryProductStates/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DeliveryProductState>> GetDeliveryProductState(int id)
        {
            var deliveryProductState = await _context.DeliveryProductState.FindAsync(id);

            if (deliveryProductState == null)
            {
                return NotFound();
            }

            return deliveryProductState;
        }

        // PUT: api/DeliveryProductStates/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDeliveryProductState(int id, DeliveryProductState deliveryProductState)
        {
            if (id != deliveryProductState.DeliveryProductStateId)
            {
                return BadRequest();
            }

            _context.Entry(deliveryProductState).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeliveryProductStateExists(id))
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

        // POST: api/DeliveryProductStates
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<DeliveryProductState>> PostDeliveryProductState(DeliveryProductState deliveryProductState)
        {
            _context.DeliveryProductState.Add(deliveryProductState);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDeliveryProductState", new { id = deliveryProductState.DeliveryProductStateId }, deliveryProductState);
        }

        // DELETE: api/DeliveryProductStates/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<DeliveryProductState>> DeleteDeliveryProductState(int id)
        {
            var deliveryProductState = await _context.DeliveryProductState.FindAsync(id);
            if (deliveryProductState == null)
            {
                return NotFound();
            }

            _context.DeliveryProductState.Remove(deliveryProductState);
            await _context.SaveChangesAsync();

            return deliveryProductState;
        }

        private bool DeliveryProductStateExists(int id)
        {
            return _context.DeliveryProductState.Any(e => e.DeliveryProductStateId == id);
        }
    }
}
