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
    public class InventoryReceivingNoteDetailForToysController : ControllerBase
    {
        private readonly PETSHOPContext _context;

        public InventoryReceivingNoteDetailForToysController(PETSHOPContext context)
        {
            _context = context;
        }

        // GET: api/InventoryReceivingNoteDetailForToys
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryReceivingNoteDetailForToy>>> GetInventoryReceivingNoteDetailForToy()
        {
            return await _context.InventoryReceivingNoteDetailForToy.ToListAsync();
        }

        // GET: api/InventoryReceivingNoteDetailForToys/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InventoryReceivingNoteDetailForToy>> GetInventoryReceivingNoteDetailForToy(int id)
        {
            var inventoryReceivingNoteDetailForToy = await _context.InventoryReceivingNoteDetailForToy.FindAsync(id);

            if (inventoryReceivingNoteDetailForToy == null)
            {
                return NotFound();
            }

            return inventoryReceivingNoteDetailForToy;
        }

        // PUT: api/InventoryReceivingNoteDetailForToys/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInventoryReceivingNoteDetailForToy(int id, InventoryReceivingNoteDetailForToy inventoryReceivingNoteDetailForToy)
        {
            if (id != inventoryReceivingNoteDetailForToy.InventoryReceivingId)
            {
                return BadRequest();
            }

            _context.Entry(inventoryReceivingNoteDetailForToy).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InventoryReceivingNoteDetailForToyExists(id))
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

        // POST: api/InventoryReceivingNoteDetailForToys
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<InventoryReceivingNoteDetailForToy>> PostInventoryReceivingNoteDetailForToy(InventoryReceivingNoteDetailForToy inventoryReceivingNoteDetailForToy)
        {
            _context.InventoryReceivingNoteDetailForToy.Add(inventoryReceivingNoteDetailForToy);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInventoryReceivingNoteDetailForToy", new { id = inventoryReceivingNoteDetailForToy.InventoryReceivingId }, inventoryReceivingNoteDetailForToy);
        }

        // DELETE: api/InventoryReceivingNoteDetailForToys/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<InventoryReceivingNoteDetailForToy>> DeleteInventoryReceivingNoteDetailForToy(int id)
        {
            var inventoryReceivingNoteDetailForToy = await _context.InventoryReceivingNoteDetailForToy.FindAsync(id);
            if (inventoryReceivingNoteDetailForToy == null)
            {
                return NotFound();
            }

            _context.InventoryReceivingNoteDetailForToy.Remove(inventoryReceivingNoteDetailForToy);
            await _context.SaveChangesAsync();

            return inventoryReceivingNoteDetailForToy;
        }

        private bool InventoryReceivingNoteDetailForToyExists(int id)
        {
            return _context.InventoryReceivingNoteDetailForToy.Any(e => e.InventoryReceivingId == id);
        }
    }
}
