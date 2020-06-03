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
    public class UserScoresController : ControllerBase
    {
        private readonly PETSHOPContext _context;

        public UserScoresController(PETSHOPContext context)
        {
            _context = context;
        }

        // GET: api/UserScores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserScore>>> GetUserScore()
        {
            return await _context.UserScore.ToListAsync();
        }

        // GET: api/UserScores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserScore>> GetUserScore(int id)
        {
            var userScore = await _context.UserScore.FindAsync(id);

            if (userScore == null)
            {
                return NotFound();
            }

            return userScore;
        }

        // PUT: api/UserScores/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserScore(int id, UserScore userScore)
        {
            if (id != userScore.UserScoreId)
            {
                return BadRequest();
            }

            _context.Entry(userScore).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserScoreExists(id))
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

        // POST: api/UserScores
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<UserScore>> PostUserScore(UserScore userScore)
        {
            _context.UserScore.Add(userScore);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserScore", new { id = userScore.UserScoreId }, userScore);
        }

        // DELETE: api/UserScores/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserScore>> DeleteUserScore(int id)
        {
            var userScore = await _context.UserScore.FindAsync(id);
            if (userScore == null)
            {
                return NotFound();
            }

            _context.UserScore.Remove(userScore);
            await _context.SaveChangesAsync();

            return userScore;
        }

        private bool UserScoreExists(int id)
        {
            return _context.UserScore.Any(e => e.UserScoreId == id);
        }
    }
}
