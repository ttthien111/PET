using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PETSHOP.Models;

namespace PETSHOP.Controllers
{
    [EnableQuery()]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountRolesController : ControllerBase
    {
        private readonly PETSHOPContext _context;

        public AccountRolesController(PETSHOPContext context)
        {
            _context = context;
        }

        // GET: api/AccountRoles
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountRole>>> GetAccountRole()
        {
            return await _context.AccountRole.ToListAsync();
        }

        // GET: api/AccountRoles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AccountRole>> GetAccountRole(int id)
        {
            var accountRole = await _context.AccountRole.FindAsync(id);

            if (accountRole == null)
            {
                return NotFound();
            }

            return accountRole;
        }

        // PUT: api/AccountRoles/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccountRole(int id, AccountRole accountRole)
        {
            if (id != accountRole.AccountRoleId)
            {
                return BadRequest();
            }

            _context.Entry(accountRole).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountRoleExists(id))
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

        // POST: api/AccountRoles
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<AccountRole>> PostAccountRole(AccountRole accountRole)
        {
            _context.AccountRole.Add(accountRole);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AccountRoleExists(accountRole.AccountRoleId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetAccountRole", new { id = accountRole.AccountRoleId }, accountRole);
        }

        // DELETE: api/AccountRoles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AccountRole>> DeleteAccountRole(int id)
        {
            var accountRole = await _context.AccountRole.FindAsync(id);
            if (accountRole == null)
            {
                return NotFound();
            }

            _context.AccountRole.Remove(accountRole);
            await _context.SaveChangesAsync();

            return accountRole;
        }

        private bool AccountRoleExists(int id)
        {
            return _context.AccountRole.Any(e => e.AccountRoleId == id);
        }
    }
}
