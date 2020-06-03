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
    public class CustomerTypesController : ControllerBase
    {
        private readonly PETSHOPContext _context;

        public CustomerTypesController(PETSHOPContext context)
        {
            _context = context;
        }

        // GET: api/CustomerTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerType>>> GetCustomerType()
        {
            return await _context.CustomerType.ToListAsync();
        }

        // GET: api/CustomerTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerType>> GetCustomerType(int id)
        {
            var customerType = await _context.CustomerType.FindAsync(id);

            if (customerType == null)
            {
                return NotFound();
            }

            return customerType;
        }

        // PUT: api/CustomerTypes/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomerType(int id, CustomerType customerType)
        {
            if (id != customerType.CustomerTypeId)
            {
                return BadRequest();
            }

            _context.Entry(customerType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerTypeExists(id))
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

        // POST: api/CustomerTypes
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<CustomerType>> PostCustomerType(CustomerType customerType)
        {
            _context.CustomerType.Add(customerType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomerType", new { id = customerType.CustomerTypeId }, customerType);
        }

        // DELETE: api/CustomerTypes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CustomerType>> DeleteCustomerType(int id)
        {
            var customerType = await _context.CustomerType.FindAsync(id);
            if (customerType == null)
            {
                return NotFound();
            }

            _context.CustomerType.Remove(customerType);
            await _context.SaveChangesAsync();

            return customerType;
        }

        private bool CustomerTypeExists(int id)
        {
            return _context.CustomerType.Any(e => e.CustomerTypeId == id);
        }
    }
}
