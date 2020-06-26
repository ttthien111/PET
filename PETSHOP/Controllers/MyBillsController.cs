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
    [Authorize(Roles = Role.Customer)]
    public class MyBillsController : ControllerBase
    {
        private readonly PETSHOPContext _context;

        public MyBillsController(PETSHOPContext context)
        {
            _context = context;
        }

        // GET: api/MyBills/5 -- get all user's bills
        [HttpGet("{userProfile}")]
        [Authorize(Roles = Role.Customer)]
        public async Task<ActionResult<IEnumerable<Bill>>> GetMyBill(int userProfile)
        {
            return await _context.Bill.Where(p => p.UserProfileId == userProfile).ToListAsync();
        }

        // GET: api/MyBills/5/10 -- get user's bill with billId = 10
        [HttpGet("{userProfile}/{id}")]
        [Authorize(Roles = Role.Customer)]
        public async Task<ActionResult<Bill>> GetBill(int userProfile, int id)
        {
            var bill = await _context.Bill.SingleOrDefaultAsync(p => p.UserProfileId == userProfile && p.BillId == id);

            if (bill == null)
            {
                return NotFound();
            }

            return bill;
        }

        // PUT: api/MyBills/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{userProfile}/{id}")]
        [Authorize(Roles = Role.Customer)]
        public async Task<IActionResult> PutMyBill(int userProfile, int id, Bill bill)
        {
            if (id == bill.BillId && bill.UserProfileId == userProfile)
            {
                if(!(_context.Bill.Find(bill.BillId).UserProfileId == userProfile))
                {
                    return Unauthorized();
                }
                else
                {
                    var local = _context.Set<Bill>()
                    .Local
                    .FirstOrDefault(entry => entry.BillId.Equals(id));

                    // check if local is not null 
                    if (local != null)
                    {
                        // detach
                        _context.Entry(local).State = EntityState.Detached;
                    }
                    // set Modified flag in your entry
                    _context.Entry(bill).State = EntityState.Modified;


                    try
                    {
                        await _context.SaveChangesAsync();
                        return NoContent();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!BillExists(id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
            }
            return Unauthorized();
        }

        // POST: api/MyBills
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("{userProfile}")]
        [Authorize(Roles = Role.Customer)]
        public async Task<ActionResult<Bill>> PostBill(int userProfile, Bill bill)
        {
            if(userProfile != bill.UserProfileId)
            {
                return BadRequest();
            }

            _context.Bill.Add(bill);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBill", new { id = bill.BillId }, bill);
        }

        private bool BillExists(int id)
        {
            return _context.Bill.Any(e => e.BillId == id);
        }
    }
}
