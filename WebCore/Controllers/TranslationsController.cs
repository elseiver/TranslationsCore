using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebCore.Models;
using System.Data.SqlClient;

namespace WebCore.Controllers
{
    [Produces("application/json")]
    [Route("api/Translations")]
    public class TranslationsController : Controller
    {
        private readonly WebCoreContext _context;

        public TranslationsController(WebCoreContext context)
        {
            _context = context;
        }

        // GET: api/Translations
        [HttpGet]
        public IEnumerable<Translation> GetTranslation()
        {
            return _context.Translation;
        }

        // GET: api/Translations/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTranslation([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var translation = await _context.Translation.SingleOrDefaultAsync(m => m.Id == id);

            if (translation == null)
            {
                return NotFound();
            }

            return Ok(translation);
        }

        // GET: api/Translations/key/5
        [HttpGet("key/{key}")]
        public async Task<IActionResult> GetTranslationsByKey([FromRoute] string key)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var translations = await (from t in _context.Translation
                                      where t.Key == key
                                      select t).ToListAsync();

            if (translations == null)
            {
                return NotFound();
            }

            return Ok(translations);
        }

        // GET: api/Translations/culture/5
        [HttpGet("culture/{culture}/{timestamp}")]
        public async Task<IActionResult> GetTranslationsByCulture([FromRoute] string culture, [FromRoute] string timestamp)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int cultureId;
            if (!int.TryParse(culture, out cultureId))
            {
                try
                {
                    cultureId = _context.Culture
                                       .FromSql("SELECT * FROM Culture WHERE Name LIKE @culture", new SqlParameter("@culture", culture))
                                       .Select(c => c.Id).FirstOrDefault();
                }
                catch(Exception ex)
                {

                }
            }

            try
            {
                timestamp = timestamp.Replace("\"", "");
            }
            catch(Exception ex)
            {

            }

            DateTime ts;
            DateTime.TryParse(timestamp, out ts);

            var translations = await (from t in _context.Translation
                                      where t.CultureId == cultureId
                                      && (ts == null || t.Modify_DT > ts)
                                      select t).ToListAsync();

            if (translations == null)
            {
                return NotFound();
            }

            var maxTimestamp = DateTime.UtcNow;
            if (translations.Any())
            {
                maxTimestamp = translations.Any(t => t.Modify_DT < DateTime.UtcNow) ? translations.Where(t => t.Modify_DT < DateTime.UtcNow).Max(t => t.Modify_DT) : DateTime.UtcNow;
            }
            return Ok(new { maxTimestamp = maxTimestamp, translations = translations.Select(t => new { t.Id, t.Key, t.Text }).ToList() });
        }

        // PUT: api/Translations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTranslation([FromRoute] int id, [FromBody] Translation translation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != translation.Id)
            {
                return BadRequest();
            }

            translation = FixCultureName(translation);
            translation.Modify_DT = DateTime.UtcNow;

            _context.Entry(translation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TranslationExists(id))
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

        // POST: api/Translations
        [HttpPost]
        public async Task<IActionResult> PostTranslation([FromBody] Translation translation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            translation = FixCultureName(translation);
            translation.Modify_DT = DateTime.UtcNow;

            _context.Translation.Add(translation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTranslation", new { id = translation.Id }, translation);
        }

        // DELETE: api/Translations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTranslation([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var translation = await _context.Translation.SingleOrDefaultAsync(m => m.Id == id);
            if (translation == null)
            {
                return NotFound();
            }

            _context.Translation.Remove(translation);
            await _context.SaveChangesAsync();

            return Ok(translation);
        }

        private bool TranslationExists(int id)
        {
            return _context.Translation.Any(e => e.Id == id);
        }

        private Translation FixCultureName(Translation translation)
        {
            if (string.IsNullOrEmpty(translation.CultureName))
            {
                var culture = _context.Culture.FirstOrDefault(c => c.Id == translation.CultureId);
                if (culture != null)
                {
                    translation.CultureName = culture.Name;
                }
            }
            return translation;
        }
    }
}