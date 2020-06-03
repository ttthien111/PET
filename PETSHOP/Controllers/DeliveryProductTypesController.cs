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
    public class DeliveryProductTypesController : ControllerBase
    {
        private readonly PETSHOPContext _context;

        public DeliveryProductTypesController(PETSHOPContext context)
        {
            _context = context;
        }

        // GET: api/DeliveryProductTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeliveryProductType>>> GetDeliveryProductType()
        {
            return await _context.DeliveryProductType.ToListAsync();
        }

        // GET: api/DeliveryProductTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DeliveryProductType>> GetDeliveryProductType(int id)
        {
            var deliveryProductType = await _context.DeliveryProductType.FindAsync(id);

            if (deliveryProductType == null)
            {
                return NotFound();
            }

            return deliveryProductType;
        }

        // PUT: api/DeliveryProductTypes/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDeliveryProductType(int id, DeliveryProductType deliveryProductType)
        {
            if (id != deliveryProductType.DeliveryProductTypeId)
            {
                return BadRequest();
            }

            _context.Entry(deliveryProductType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeliveryProductTypeExists(id))
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

        // POST: api/DeliveryProductTypes
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<DeliveryProductType>> PostDeliveryProductType(DeliveryProductType deliveryProductType)
        {
            _context.DeliveryProductType.Add(deliveryProductType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDeliveryProductType", new { id = deliveryProductType.DeliveryProductTypeId }, deliveryProductType);
        }

        // DELETE: api/DeliveryProductTypes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<DeliveryProductType>> DeleteDeliveryProductType(int id)
        {
            var deliveryProductType = await _context.DeliveryProductType.FindAsync(id);
            if (deliveryProductType == null)
            {
                return NotFound();
            }

            _context.DeliveryProductType.Remove(deliveryProductType);
            await _context.SaveChangesAsync();

            return deliveryProductType;
        }

        private bool DeliveryProductTypeExists(int id)
        {
            return _context.DeliveryProductType.Any(e => e.DeliveryProductTypeId == id);
        }
    }
}
