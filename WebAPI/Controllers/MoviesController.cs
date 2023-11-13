using WebAPI.Modeles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Modeles;

namespace apiWebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MovieContext _DbContext;
        public MoviesController(MovieContext dbContext)
        {
            _DbContext = dbContext;
        }
        //GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            if (_DbContext.Movies == null)
            {
                return NotFound();
            }
            return await _DbContext.Movies.ToListAsync();
        }
        //GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            if (_DbContext.Movies == null)
            {
                return NotFound();
            }
            var movie = await _DbContext.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return movie;
        }
        //POST:api/Movies
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(Movie movie)
        {
            _DbContext.Movies.Add(movie);
            await _DbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);
        }
        // PUT: api/Movies/5 [HttpPut("{id}")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, Movie movie)
        {

            if (id != movie.Id)
            {

                return BadRequest();
            }

            _DbContext.Entry(movie).State = EntityState.Modified;

            try
            {

                await _DbContext.SaveChangesAsync();
            }

            catch (DbUpdateConcurrencyException)
            {

                if (!MovieExists(id))
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
        // DELETE: api/Movies/5
        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteMovie(int id)
        {
            if (_DbContext.Movies == null)
            {
                return NotFound();
            }

            var movie = await _DbContext.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            _DbContext.Movies.Remove(movie);
            await _DbContext.SaveChangesAsync();

            return NoContent();
        }

        private bool MovieExists(long id)
        {

            return (_DbContext.Movies?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}