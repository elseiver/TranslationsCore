using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebCore.Models;
using System.Data.SqlClient;
using System.Collections.Concurrent;

namespace WebCore.Controllers
{
    [Produces("application/json")]
    [Route("api/AutoComplete")]
    public class AutoCompleteController : Controller
    {
        private static int MaxResults = 10;
        private readonly WebCoreContext _context;
        private static Object lockObject = new Object();

        private static ConcurrentDictionary<int, TrieLib.Trie> _cultureTries = new ConcurrentDictionary<int, TrieLib.Trie>();
        private TrieLib.Trie GetTrie(int cultureId)
        {
            if (!_cultureTries.ContainsKey(cultureId))
            {
                var translations = (from t in _context.Translation
                                    where t.CultureId == cultureId
                                    select t).ToList();
                var entries = new Dictionary<int, string>();
                translations.ForEach(translation => entries.Add(translation.Id, translation.Text.ToLower()));
                lock (lockObject)
                {
                    var cultureTrie = new TrieLib.Trie();
                    cultureTrie.InsertSet(entries);
                    _cultureTries.TryAdd(cultureId, cultureTrie);
                }
            }

            TrieLib.Trie tree;
            if (_cultureTries.TryGetValue(cultureId, out tree))
            {
                return tree;
            }
            return new TrieLib.Trie();
        }

        public AutoCompleteController(WebCoreContext context)
        {
            _context = context;
        }

        [HttpGet("{prefix}")]
        public async Task<IActionResult> GetSuggestions([FromRoute] string prefix)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TrieLib.Trie trie = GetTrie(1);

            var suggestionIds = trie.Search(prefix);

            var suggestions = await (from t in _context.Translation
                                     where suggestionIds.Contains(t.Id)
                                     select t.Text).Take(MaxResults).ToListAsync();

            if (suggestions == null)
            {
                return NotFound();
            }

            return Ok(suggestions);
        }
    }
}