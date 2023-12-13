using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SCP.Context;
using SCP.Entities;

namespace SCP.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]

    public class ArticleController : ControllerBase
    {
        private readonly MyDbContext _context;

        public ArticleController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<article>>> GetArticles()
        {
            return await _context.articles.ToListAsync();
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<article>> GetArticle(int id)
        {
            var article = await _context.articles.FindAsync(id);

            if (article == null)
            {
                return NotFound();
            }

            return article;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutArticle(int id, article article)
        {
            if (id != article.articleID)
            {
                return BadRequest();
            }

            _context.articles.Update(article);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!articleExists(id))
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
        
        [HttpPost]
        public async Task<ActionResult<article>> PostArticle(article article)
        {
            _context.articles.Add(article);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (articleExists(article.articleID))
                {
                    return (Conflict());
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetArticle", new { id = article.articleID }, article);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            var article = await _context.articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            _context.articles.Remove(article);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool articleExists(int id)
        {
            return _context.articles.Any(e => e.articleID == id);
        }

    }
}