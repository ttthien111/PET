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
    public class InventoryReceivingNoteDetailForCostumesController : ControllerBase
    {
        private readonly PETSHOPContext _context;

        public InventoryReceivingNoteDetailForCostumesController(PETSHOPContext context)
        {
            _context = context;
        }

        // GET: api/InventoryReceivingNoteDetailForCostumes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryReceivingNoteDetailForCostume>>> GetInventoryReceivingNoteDetailForCostume()
        {
            return await _context.InventoryReceivingNoteDetailForCostume.ToListAsync();
        }

        // GET: api/InventoryReceivingNoteDetailForCostumes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InventoryReceivingNoteDetailForCostume>> GetInventoryReceivingNoteDetailForCostume(int id)
        {
            var inventoryReceivingNoteDetailForCostume = await _context.InventoryReceivingNoteDetailForCostume.FindAsync(id);

            if (inventoryReceivingNoteDetailForCostume == null)
            {
                return NotFound();
            }

            return inventoryReceivingNoteDetailForCostume;
        }

        // PUT: api/InventoryReceivingNoteDetailForCostumes/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInventoryReceivingNoteDetailForCostume(int id, InventoryReceivingNoteDetailForCostume inventoryReceivingNoteDetailForCostume)
        {
            if (id != inventoryReceivingNoteDetailForCostume.InventoryReceivingId)
            {
                return BadRequest();
            }

            _context.Entry(inventoryReceivingNoteDetailForCostume).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InventoryReceivingNoteDetailForCostumeExists(id))
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

        // POST: api/InventoryReceivingNoteDetailForCostumes
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<InventoryReceivingNoteDetailForCostume>> PostInventoryReceivingNoteDetailForCostume(InventoryReceivingNoteDetailForCostume inventoryReceivingNoteDetailForCostume)
        {
            _context.InventoryReceivingNoteDetailForCostume.Add(inventoryReceivingNoteDetailForCostume);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInventoryReceivingNoteDetailForCostume", new { id = inventoryReceivingNoteDetailForCostume.InventoryReceivingId }, inventoryReceivingNoteDetailForCostume);
        }

        // DELETE: api/InventoryReceivingNoteDetailForCostumes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<InventoryReceivingNoteDetailForCostume>> DeleteInventoryReceivingNoteDetailForCostume(int id)
        {
            var inventoryReceivingNoteDetailForCostume = await _context.InventoryReceivingNoteDetailForCostume.FindAsync(id);
            if (inventoryReceivingNoteDetailForCostume == null)
            {
                return NotFound();
            }

            _context.InventoryReceivingNoteDetailForCostume.Remove(inventoryReceivingNoteDetailForCostume);
            await _context.SaveChangesAsync();

            return inventoryReceivingNoteDetailForCostume;
        }

        private bool InventoryReceivingNoteDetailForCostumeExists(int id)
        {
            return _context.InventoryReceivingNoteDetailForCostume.Any(e => e.InventoryReceivingId == id);
        }
    }
}
