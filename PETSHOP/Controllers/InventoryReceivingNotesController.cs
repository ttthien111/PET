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
    public class InventoryReceivingNotesController : ControllerBase
    {
        private readonly PETSHOPContext _context;

        public InventoryReceivingNotesController(PETSHOPContext context)
        {
            _context = context;
        }

        // GET: api/InventoryReceivingNotes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryReceivingNote>>> GetInventoryReceivingNote()
        {
            return await _context.InventoryReceivingNote.ToListAsync();
        }

        // GET: api/InventoryReceivingNotes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InventoryReceivingNote>> GetInventoryReceivingNote(int id)
        {
            var inventoryReceivingNote = await _context.InventoryReceivingNote.FindAsync(id);

            if (inventoryReceivingNote == null)
            {
                return NotFound();
            }

            return inventoryReceivingNote;
        }

        // PUT: api/InventoryReceivingNotes/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInventoryReceivingNote(int id, InventoryReceivingNote inventoryReceivingNote)
        {
            if (id != inventoryReceivingNote.InventoryReceivingId)
            {
                return BadRequest();
            }

            _context.Entry(inventoryReceivingNote).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InventoryReceivingNoteExists(id))
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

        // POST: api/InventoryReceivingNotes
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<InventoryReceivingNote>> PostInventoryReceivingNote(InventoryReceivingNote inventoryReceivingNote)
        {
            _context.InventoryReceivingNote.Add(inventoryReceivingNote);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInventoryReceivingNote", new { id = inventoryReceivingNote.InventoryReceivingId }, inventoryReceivingNote);
        }

        // DELETE: api/InventoryReceivingNotes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<InventoryReceivingNote>> DeleteInventoryReceivingNote(int id)
        {
            var inventoryReceivingNote = await _context.InventoryReceivingNote.FindAsync(id);
            if (inventoryReceivingNote == null)
            {
                return NotFound();
            }

            _context.InventoryReceivingNote.Remove(inventoryReceivingNote);
            await _context.SaveChangesAsync();

            return inventoryReceivingNote;
        }

        private bool InventoryReceivingNoteExists(int id)
        {
            return _context.InventoryReceivingNote.Any(e => e.InventoryReceivingId == id);
        }
    }
}
