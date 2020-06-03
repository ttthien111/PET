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
    public class CostumeProductsController : ControllerBase
    {
        private readonly PETSHOPContext _context;

        public CostumeProductsController(PETSHOPContext context)
        {
            _context = context;
        }

        // GET: api/CostumeProducts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CostumeProduct>>> GetCostumeProduct()
        {
            return await _context.CostumeProduct.ToListAsync();
        }

        // GET: api/CostumeProducts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CostumeProduct>> GetCostumeProduct(int id)
        {
            var costumeProduct = await _context.CostumeProduct.FindAsync(id);

            if (costumeProduct == null)
            {
                return NotFound();
            }

            return costumeProduct;
        }

        // PUT: api/CostumeProducts/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCostumeProduct(int id, CostumeProduct costumeProduct)
        {
            if (id != costumeProduct.CostumeId)
            {
                return BadRequest();
            }

            _context.Entry(costumeProduct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CostumeProductExists(id))
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

        // POST: api/CostumeProducts
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<CostumeProduct>> PostCostumeProduct(CostumeProduct costumeProduct)
        {
            _context.CostumeProduct.Add(costumeProduct);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCostumeProduct", new { id = costumeProduct.CostumeId }, costumeProduct);
        }

        // DELETE: api/CostumeProducts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CostumeProduct>> DeleteCostumeProduct(int id)
        {
            var costumeProduct = await _context.CostumeProduct.FindAsync(id);
            if (costumeProduct == null)
            {
                return NotFound();
            }

            _context.CostumeProduct.Remove(costumeProduct);
            await _context.SaveChangesAsync();

            return costumeProduct;
        }

        private bool CostumeProductExists(int id)
        {
            return _context.CostumeProduct.Any(e => e.CostumeId == id);
        }
    }
}
