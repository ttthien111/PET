using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PETSHOP.Helper;
using PETSHOP.Models;
using PETSHOP.Models.LoginModel;

namespace PETSHOP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Role.Admin + "," + Role.User)]
    public class AccountManagesController : ControllerBase
    {
        private readonly PETSHOPContext _context;
        
        public AccountManagesController(PETSHOPContext context)
        {
            _context = context;
        }
       
        // GET: api/AccountManages
        [HttpGet]
        [Authorize(Roles = Role.Admin + "," + Role.User)]
        public async Task<ActionResult<IEnumerable<AccountManage>>> GetAccountManage()
        {
            return await _context.AccountManage.ToListAsync();
        }

        // GET: api/AccountManages/5
        [HttpGet("{id}")]
        [Authorize(Roles = Role.Admin + "," + Role.User)]
        public async Task<ActionResult<AccountManage>> GetAccountManage(string id)
        {
           var accountManage = await _context.AccountManage.FindAsync(id);

            if (accountManage == null)
            {
                return NotFound();
            }

            return accountManage;
        }

        // PUT: api/AccountManages/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Authorize(Roles = Role.Admin + "," + Role.User)]
        public async Task<IActionResult> PutAccountManage(string id, AccountManage accountManage)
        {
            if (id != accountManage.Email)
            {
                return BadRequest();
            }

            _context.Entry(accountManage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountManageExists(id))
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

        // POST: api/AccountManages
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public async Task<ActionResult<AccountManage>> PostAccountManage(AccountManage accountManage)
        {
            _context.AccountManage.Add(accountManage);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AccountManageExists(accountManage.Email))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetAccountManage", new { id = accountManage.Email }, accountManage);
        }

        // DELETE: api/AccountManages/5
        [HttpDelete("{id}")]
        [Authorize(Roles = Role.Admin)]
        public async Task<ActionResult<AccountManage>> DeleteAccountManage(string id)
        {
            var accountManage = await _context.AccountManage.FindAsync(id);
            if (accountManage == null)
            {
                return NotFound();
            }

            _context.AccountManage.Remove(accountManage);
            await _context.SaveChangesAsync();

            return accountManage;
        }

        private bool AccountManageExists(string id)
        {
            return _context.AccountManage.Any(e => e.Email == id);
        }
    }
}
