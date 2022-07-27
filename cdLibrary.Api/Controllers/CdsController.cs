using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using cdLibrary.Api.Models;
using cdLibrary.Api.Data;

namespace cdLibrary.Api.Controllers;

[ApiController]
[Route("/api/[Controller]")]
public class CdsController : Controller
{
    private readonly CdContext _context;

    public CdsController(CdContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<CdDto>> GetCd(int id)
    {
        var cd = await _context.Cd.Include(c => c.Genre).Where(c => c.Id == id).Select(c =>
            new CdDto()
            {
                Title = c.Title,
                Artist = c.Artist,
                Description = c.Description,
                Genre = c.Genre.Name,
                PurchaseDate = c.PurchaseDate
            }).SingleOrDefaultAsync();

        if (cd == null)
        {
            return NotFound();
        }

        return Ok(cd);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CdDto>>> GetCds(string? genre)
    {
        var query = _context.Cd.AsQueryable();

        if (!String.IsNullOrEmpty(genre))
        {
            query = query.Where(c => c.Genre.Name.Contains(genre));
        }

        var dto = from c in query
                  select new CdDto()
                  {
                      Title = c.Title,
                      Artist = c.Artist,
                      Description = c.Description,
                      Genre = c.Genre.Name,
                      PurchaseDate = c.PurchaseDate
                  };

        var cdsDto = await dto.ToListAsync();

        return Ok(cdsDto);
    }

    [HttpPost]
    public async Task<ActionResult<Cd>> Create(CdDto dto)
    {
        var newCd = new Cd()
        {
            Title = dto.Title,
            Artist = dto.Artist,
            Description = dto.Description,
            PurchaseDate = dto.PurchaseDate
        };

        var existingGenre = _context.Genre.Where(c => c.Name == dto.Genre).FirstOrDefault();

        if (existingGenre != null)
        {
            newCd.Genre = existingGenre;
        }
        else
        {
            newCd.Genre = new Genre();
            newCd.Genre.Name = dto.Genre;
        }

        await _context.AddAsync(newCd);
        await _context.SaveChangesAsync();

        _context.Entry(newCd).Reference(x => x.Genre).Load();

        return CreatedAtAction(nameof(GetCd), new { id = newCd.Id }, newCd);
    }

    [HttpPut]
    [Route("{id}/artist")]
    public async Task<ActionResult<Cd>> AddArtist(int id, string artist)
    {
        var cd = await _context.Cd.FindAsync(id);

        if (cd == null)
        {
            return NotFound();
        }
        cd.Artist = artist;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPut]
    [Route("{id}/genre")]
    public async Task<ActionResult> AddGenre(int id, Genre genre)
    {
        var cd = await _context.Cd.FindAsync(id);

        if (cd == null)
        {
            return NotFound();
        }

        var existingGenre = _context.Genre.Where(c => c.Name == genre.Name).FirstOrDefault();

        cd.Genre = existingGenre == null ? genre : existingGenre;

        await _context.SaveChangesAsync();

        return NoContent();
    }
    [HttpDelete]
    public async Task<ActionResult> RemoveCd(int id)
    {
        var cd = await _context.Cd.FindAsync(id);

        if (cd == null)
        {
            return NotFound();
        }

        _context.Remove(cd);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

