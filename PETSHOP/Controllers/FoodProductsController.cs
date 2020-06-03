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
    public class FoodProductsController : ControllerBase
    {
        private readonly PETSHOPContext _context;

        public FoodProductsController(PETSHOPContext context)
        {
            _context = context;
        }

        // GET: api/FoodProducts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FoodProduct>>> GetFoodProduct()
        {
            return await _context.FoodProduct.ToListAsync();
        }

        // GET: api/FoodProducts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FoodProduct>> GetFoodProduct(int id)
        {
            var foodProduct = await _context.FoodProduct.FindAsync(id);

            if (foodProduct == null)
            {
                return NotFound();
            }

            return foodProduct;
        }

        // PUT: api/FoodProducts/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFoodProduct(int id, FoodProduct foodProduct)
        {
            if (id != foodProduct.FoodId)
            {
                return BadRequest();
            }

            _context.Entry(foodProduct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FoodProductExists(id))
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

        // POST: api/FoodProducts
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<FoodProduct>> PostFoodProduct(FoodProduct foodProduct)
        {
            _context.FoodProduct.Add(foodProduct);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFoodProduct", new { id = foodProduct.FoodId }, foodProduct);
        }

        // DELETE: api/FoodProducts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<FoodProduct>> DeleteFoodProduct(int id)
        {
            var foodProduct = await _context.FoodProduct.FindAsync(id);
            if (foodProduct == null)
            {
                return NotFound();
            }

            _context.FoodProduct.Remove(foodProduct);
            await _context.SaveChangesAsync();

            return foodProduct;
        }

        private bool FoodProductExists(int id)
        {
            return _context.FoodProduct.Any(e => e.FoodId == id);
        }
    }
}
