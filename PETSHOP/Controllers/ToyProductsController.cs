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
    public class ToyProductsController : ControllerBase
    {
        private readonly PETSHOPContext _context;

        public ToyProductsController(PETSHOPContext context)
        {
            _context = context;
        }

        // GET: api/ToyProducts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToyProduct>>> GetToyProduct()
        {
            return await _context.ToyProduct.ToListAsync();
        }

        // GET: api/ToyProducts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ToyProduct>> GetToyProduct(int id)
        {
            var toyProduct = await _context.ToyProduct.FindAsync(id);

            if (toyProduct == null)
            {
                return NotFound();
            }

            return toyProduct;
        }

        // PUT: api/ToyProducts/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutToyProduct(int id, ToyProduct toyProduct)
        {
            if (id != toyProduct.ToyId)
            {
                return BadRequest();
            }

            _context.Entry(toyProduct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToyProductExists(id))
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

        // POST: api/ToyProducts
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<ToyProduct>> PostToyProduct(ToyProduct toyProduct)
        {
            _context.ToyProduct.Add(toyProduct);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetToyProduct", new { id = toyProduct.ToyId }, toyProduct);
        }

        // DELETE: api/ToyProducts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ToyProduct>> DeleteToyProduct(int id)
        {
            var toyProduct = await _context.ToyProduct.FindAsync(id);
            if (toyProduct == null)
            {
                return NotFound();
            }

            _context.ToyProduct.Remove(toyProduct);
            await _context.SaveChangesAsync();

            return toyProduct;
        }

        private bool ToyProductExists(int id)
        {
            return _context.ToyProduct.Any(e => e.ToyId == id);
        }
    }
}
