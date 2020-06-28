using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
        private IHttpContextAccessor _httpContextAccessor;
        public MyBillsController(PETSHOPContext context,IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: api/MyBills/5 -- get all user's bills
        [Authorize(Roles = Role.Customer)]
        public async Task<ActionResult<IEnumerable<Bill>>> GetMyBill()
        {
            int profileId = GetProfileId();
            if(profileId != 0)
                return await _context.Bill.Where(p => p.UserProfileId == profileId).ToListAsync();
            return Unauthorized();
        }

        // GET: api/MyBills/5/10 -- get user's bill with billId = 10
        [HttpGet("{id}")]
        [Authorize(Roles = Role.Customer)]
        public async Task<ActionResult<Bill>> GetBill(int id)
        {
            var bill = await _context.Bill.SingleOrDefaultAsync(p => p.UserProfileId == GetProfileId() && p.BillId == id);

            if (bill == null)
            {
                return NotFound();
            }

            return bill;
        }

        // PUT: api/MyBills/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Authorize(Roles = Role.Customer)]
        public async Task<IActionResult> PutMyBill(int id, Bill bill)
        {
            if (id == bill.BillId && bill.UserProfileId == GetProfileId())
            {
                if(!(_context.Bill.Find(bill.BillId).UserProfileId == GetProfileId()))
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
        [HttpPost]
        [Authorize(Roles = Role.Customer)]
        public async Task<ActionResult<Bill>> PostBill([FromBody] Bill bill)
        {
            if(GetProfileId() == bill.UserProfileId)
            {
                _context.Bill.Add(bill);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetBill", new { id = bill.BillId }, bill);
            }
            else
            {
                return BadRequest();
            }
        }

        private bool BillExists(int id)
        {
            return _context.Bill.Any(e => e.BillId == id);
        }

        private int GetProfileId()
        {
            string header = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            string token = header.Split(" ")[1];

            // decode jwt
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadJwtToken(token);

            var accountId = tokenS.Claims.SingleOrDefault(p => p.Type == "unique_name").Value;

            return _context.UserProfile.SingleOrDefault(p => p.AccountId == int.Parse(accountId)).UserProfileId;
        }
    }
}
