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
    public class DistributorsController : ControllerBase
    {
        private readonly PETSHOPContext _context;

        public DistributorsController(PETSHOPContext context)
        {
            _context = context;
        }

        // GET: api/Distributors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Distributor>>> GetDistributor()
        {
            return await _context.Distributor.ToListAsync();
        }

        // GET: api/Distributors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Distributor>> GetDistributor(int id)
        {
            var distributor = await _context.Distributor.FindAsync(id);

            if (distributor == null)
            {
                return NotFound();
            }

            return distributor;
        }

        // PUT: api/Distributors/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDistributor(int id, Distributor distributor)
        {
            if (id != distributor.DistributorId)
            {
                return BadRequest();
            }

            _context.Entry(distributor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DistributorExists(id))
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

        // POST: api/Distributors
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Distributor>> PostDistributor(Distributor distributor)
        {
            _context.Distributor.Add(distributor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDistributor", new { id = distributor.DistributorId }, distributor);
        }

        // DELETE: api/Distributors/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Distributor>> DeleteDistributor(int id)
        {
            var distributor = await _context.Distributor.FindAsync(id);
            if (distributor == null)
            {
                return NotFound();
            }

            _context.Distributor.Remove(distributor);
            await _context.SaveChangesAsync();

            return distributor;
        }

        private bool DistributorExists(int id)
        {
            return _context.Distributor.Any(e => e.DistributorId == id);
        }
    }
}
