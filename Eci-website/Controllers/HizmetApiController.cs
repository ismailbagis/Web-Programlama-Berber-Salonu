using Microsoft.AspNetCore.Mvc;
using Eci_website.Models;
using Microsoft.EntityFrameworkCore;

namespace Eci_website.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HizmetApiController : ControllerBase
    {
        private readonly IdentityContext _context;

        public HizmetApiController(IdentityContext context)
        {
            _context = context;
        }

        // GET: api/Hizmet
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hizmet>>> GetHizmetler()
        {
            return await _context.Hizmetler.Include(h => h.Salon).ToListAsync();
        }

        // GET: api/Hizmet/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Hizmet>> GetHizmet(int id)
        {
            var hizmet = await _context.Hizmetler.FindAsync(id);

            if (hizmet == null)
            {
                return NotFound();
            }

            return hizmet;
        }

        // POST: api/Hizmet
        [HttpPost]
        public async Task<ActionResult<Hizmet>> PostHizmet(Hizmet hizmet)
        {
            _context.Hizmetler.Add(hizmet);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHizmet", new { id = hizmet.Id }, hizmet);
        }

        // PUT: api/Hizmet/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHizmet(int id, Hizmet hizmet)
        {
            if (id != hizmet.Id)
            {
                return BadRequest();
            }

            _context.Entry(hizmet).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Hizmet/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHizmet(int id)
        {
            var hizmet = await _context.Hizmetler.FindAsync(id);
            if (hizmet == null)
            {
                return NotFound();
            }

            _context.Hizmetler.Remove(hizmet);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
