using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PETSHOP.Models;
using PETSHOP.Models.LoginModel;

namespace PETSHOP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly PETSHOPContext _context;

        public ProductsController(PETSHOPContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductBySlugCategory()
        {
            return await _context.Product.ToListAsync();
        }

        // GET: api/Products/food
        [HttpGet("{slugCat}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductBySlugCategory(string slugCat)
        {
            var category = await _context.Category.SingleOrDefaultAsync(p => p.CategoryName == slugCat);
            return await _context.Product.Where(p => p.CategoryId == category.CategoryId).ToListAsync();
        }

        // GET: api/Products/slugCat/5
        [HttpGet("{slugCat}/{slugName}")]
        public async Task<ActionResult<Product>> GetProductByCategoryAndId(string slugCat, string slugName)
        {
            var category = await _context.Category.SingleOrDefaultAsync(p => p.CategoryName == slugCat);
            var product = await _context.Product.SingleOrDefaultAsync(p=>p.CategoryId ==  category.CategoryId && p.SlugName == slugName);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        [HttpGet("category/{nametype}")]
        public async Task<ActionResult<IEnumerable<Product>>> CategoryProduct(string nametype = "costume")
        {
            if (!String.IsNullOrEmpty(nametype))
            {
                if(nametype == "food")
                {
                    return await _context.Product.Where(p=> p.CategoryId == 1).ToListAsync();
                }
                else if (nametype == "toys")
                {
                    return await _context.Product.Where(p => p.CategoryId == 2).ToListAsync();
                }
                else 
                {
                    return await _context.Product.Where(p => p.CategoryId == 3).ToListAsync();
                }
            }
            return NotFound();
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [Authorize(Roles=Role.Admin + "," + Role.User)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // POST: api/Products
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [Authorize(Roles = Role.Admin + "," + Role.User)]
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Product.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductByCategoryAndId", new { slugCat = product.CategoryId, slugName = product.SlugName }, product);
        }

        // DELETE: api/Products/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Product.Remove(product);
            await _context.SaveChangesAsync();

            return product;
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }
    }
}
