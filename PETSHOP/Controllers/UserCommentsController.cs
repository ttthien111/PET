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
    public class UserCommentsController : ControllerBase
    {
        private readonly PETSHOPContext _context;

        public UserCommentsController(PETSHOPContext context)
        {
            _context = context;
        }

        // GET: api/UserComments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserComment>>> GetUserComment()
        {
            return await _context.UserComment.ToListAsync();
        }

        // GET: api/UserComments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserComment>> GetUserComment(int id)
        {
            var userComment = await _context.UserComment.FindAsync(id);

            if (userComment == null)
            {
                return NotFound();
            }

            return userComment;
        }

        // PUT: api/UserComments/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserComment(int id, UserComment userComment)
        {
            if (id != userComment.UserCommentId)
            {
                return BadRequest();
            }

            _context.Entry(userComment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserCommentExists(id))
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

        // POST: api/UserComments
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<UserComment>> PostUserComment(UserComment userComment)
        {
            _context.UserComment.Add(userComment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserComment", new { id = userComment.UserCommentId }, userComment);
        }

        // DELETE: api/UserComments/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserComment>> DeleteUserComment(int id)
        {
            var userComment = await _context.UserComment.FindAsync(id);
            if (userComment == null)
            {
                return NotFound();
            }

            _context.UserComment.Remove(userComment);
            await _context.SaveChangesAsync();

            return userComment;
        }

        private bool UserCommentExists(int id)
        {
            return _context.UserComment.Any(e => e.UserCommentId == id);
        }
    }
}
