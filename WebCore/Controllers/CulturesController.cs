using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebCore.Models;

namespace WebCore.Controllers
{
    [Produces("application/json")]
    [Route("api/Cultures")]
    public class CulturesController : Controller
    {
        private readonly WebCoreContext _context;

        public CulturesController(WebCoreContext context)
        {
            _context = context;
        }

        // GET: api/Cultures
        [HttpGet]
        public IEnumerable<Culture> GetCulture()
        {
            return _context.Culture;
        }

        // GET: api/Cultures/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCulture([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var culture = await _context.Culture.SingleOrDefaultAsync(m => m.Id == id);

            if (culture == null)
            {
                return NotFound();
            }

            return Ok(culture);
        }

        // PUT: api/Cultures/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCulture([FromRoute] int id, [FromBody] Culture culture)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != culture.Id)
            {
                return BadRequest();
            }

            _context.Entry(culture).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CultureExists(id))
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

        // POST: api/Cultures
        [HttpPost]
        public async Task<IActionResult> PostCulture([FromBody] Culture culture)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {


                _context.Culture.Add(culture);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {


            }
            return CreatedAtAction("GetCulture", new { id = culture.Id }, culture);
        }

        // DELETE: api/Cultures/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCulture([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var culture = await _context.Culture.SingleOrDefaultAsync(m => m.Id == id);
            if (culture == null)
            {
                return NotFound();
            }

            _context.Culture.Remove(culture);
            await _context.SaveChangesAsync();

            return Ok(culture);
        }

        private bool CultureExists(int id)
        {
            return _context.Culture.Any(e => e.Id == id);
        }
    }
}