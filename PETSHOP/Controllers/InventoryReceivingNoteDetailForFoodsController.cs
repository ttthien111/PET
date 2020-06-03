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
    public class InventoryReceivingNoteDetailForFoodsController : ControllerBase
    {
        private readonly PETSHOPContext _context;

        public InventoryReceivingNoteDetailForFoodsController(PETSHOPContext context)
        {
            _context = context;
        }

        // GET: api/InventoryReceivingNoteDetailForFoods
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryReceivingNoteDetailForFood>>> GetInventoryReceivingNoteDetailForFood()
        {
            return await _context.InventoryReceivingNoteDetailForFood.ToListAsync();
        }

        // GET: api/InventoryReceivingNoteDetailForFoods/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InventoryReceivingNoteDetailForFood>> GetInventoryReceivingNoteDetailForFood(int id)
        {
            var inventoryReceivingNoteDetailForFood = await _context.InventoryReceivingNoteDetailForFood.FindAsync(id);

            if (inventoryReceivingNoteDetailForFood == null)
            {
                return NotFound();
            }

            return inventoryReceivingNoteDetailForFood;
        }

        // PUT: api/InventoryReceivingNoteDetailForFoods/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInventoryReceivingNoteDetailForFood(int id, InventoryReceivingNoteDetailForFood inventoryReceivingNoteDetailForFood)
        {
            if (id != inventoryReceivingNoteDetailForFood.InventoryReceivingId)
            {
                return BadRequest();
            }

            _context.Entry(inventoryReceivingNoteDetailForFood).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InventoryReceivingNoteDetailForFoodExists(id))
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

        // POST: api/InventoryReceivingNoteDetailForFoods
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<InventoryReceivingNoteDetailForFood>> PostInventoryReceivingNoteDetailForFood(InventoryReceivingNoteDetailForFood inventoryReceivingNoteDetailForFood)
        {
            _context.InventoryReceivingNoteDetailForFood.Add(inventoryReceivingNoteDetailForFood);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInventoryReceivingNoteDetailForFood", new { id = inventoryReceivingNoteDetailForFood.InventoryReceivingId }, inventoryReceivingNoteDetailForFood);
        }

        // DELETE: api/InventoryReceivingNoteDetailForFoods/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<InventoryReceivingNoteDetailForFood>> DeleteInventoryReceivingNoteDetailForFood(int id)
        {
            var inventoryReceivingNoteDetailForFood = await _context.InventoryReceivingNoteDetailForFood.FindAsync(id);
            if (inventoryReceivingNoteDetailForFood == null)
            {
                return NotFound();
            }

            _context.InventoryReceivingNoteDetailForFood.Remove(inventoryReceivingNoteDetailForFood);
            await _context.SaveChangesAsync();

            return inventoryReceivingNoteDetailForFood;
        }

        private bool InventoryReceivingNoteDetailForFoodExists(int id)
        {
            return _context.InventoryReceivingNoteDetailForFood.Any(e => e.InventoryReceivingId == id);
        }
    }
}
