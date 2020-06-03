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
    public class DeliveryProductsController : ControllerBase
    {
        private readonly PETSHOPContext _context;

        public DeliveryProductsController(PETSHOPContext context)
        {
            _context = context;
        }

        // GET: api/DeliveryProducts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeliveryProduct>>> GetDeliveryProduct()
        {
            return await _context.DeliveryProduct.ToListAsync();
        }

        // GET: api/DeliveryProducts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DeliveryProduct>> GetDeliveryProduct(int id)
        {
            var deliveryProduct = await _context.DeliveryProduct.FindAsync(id);

            if (deliveryProduct == null)
            {
                return NotFound();
            }

            return deliveryProduct;
        }

        // PUT: api/DeliveryProducts/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDeliveryProduct(int id, DeliveryProduct deliveryProduct)
        {
            if (id != deliveryProduct.DeliveryProductId)
            {
                return BadRequest();
            }

            _context.Entry(deliveryProduct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeliveryProductExists(id))
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

        // POST: api/DeliveryProducts
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<DeliveryProduct>> PostDeliveryProduct(DeliveryProduct deliveryProduct)
        {
            _context.DeliveryProduct.Add(deliveryProduct);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDeliveryProduct", new { id = deliveryProduct.DeliveryProductId }, deliveryProduct);
        }

        // DELETE: api/DeliveryProducts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<DeliveryProduct>> DeleteDeliveryProduct(int id)
        {
            var deliveryProduct = await _context.DeliveryProduct.FindAsync(id);
            if (deliveryProduct == null)
            {
                return NotFound();
            }

            _context.DeliveryProduct.Remove(deliveryProduct);
            await _context.SaveChangesAsync();

            return deliveryProduct;
        }

        private bool DeliveryProductExists(int id)
        {
            return _context.DeliveryProduct.Any(e => e.DeliveryProductId == id);
        }
    }
}
