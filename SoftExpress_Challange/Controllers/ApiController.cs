using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftExpress_Challange.Models;
using SoftExpress_Challange.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
public class ApiController : ControllerBase
{
    private readonly AppDbContext _context;

    public ApiController(AppDbContext context)
    {
        _context = context;
    }
    [Route("api/actions")]
    [HttpGet]
    public async Task<IActionResult> GetAllStatistikas(
    string? userId = null,
    string? rajoni = null,
    string? llojiIPajisjes = null,
    string? aplikacioni = null,
    DateTime? dataOra = null)
    {
        // Build the query with optional filters
        var query = _context.Statistikas.Include(s => s.User).AsQueryable();

        if (!string.IsNullOrEmpty(rajoni))
        {
            query = query.Where(s => s.Rajoni == rajoni);
        }

        if (!string.IsNullOrEmpty(llojiIPajisjes))
        {
            query = query.Where(s => s.Lloji_I_Pajisjes == llojiIPajisjes);
        }

        if (!string.IsNullOrEmpty(aplikacioni))
        {
            query = query.Where(s => s.Aplikacioni == aplikacioni);
        }

        if (dataOra.HasValue)
        {
            query = query.Where(s => s.DataOra.Date == dataOra.Value.Date);
        }

        var statistikate = await query.ToListAsync();

        // Map to DTO
        var dtoList = statistikate.Select(s => new StatistikaDto
        {
            Id = s.Id,
            Username = s.User != null ? s.User.Emri : "Unknown",
            UserId = s.UserId,
            Lloji_I_Pajisjes = s.Lloji_I_Pajisjes,
            Rajoni = s.Rajoni,
            Aplikacioni = s.Aplikacioni,
            DataOra = s.DataOra
        }).ToList();

        return Ok(dtoList);
    }

    [Route("api/actions")]
    [HttpPost]
    public async Task<IActionResult> CreateStatistika([FromBody] StatistikaCreateDto model)
    {
        if (model == null || !ModelState.IsValid)
        {
            return BadRequest(ModelState); // Returns the validation errors
        }

        try
        {
            var newStatistika = new Statistika
            {
                Id = Guid.NewGuid(),
                UserId = model.UserId,
                Lloji_I_Pajisjes = model.Lloji_I_Pajisjes,
                Rajoni = model.Rajoni,
                Aplikacioni = model.Aplikacioni,
                DataOra = DateTime.Now,
            };

            _context.Statistikas.Add(newStatistika);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Statistika created successfully." });
        }
        catch (Exception ex)
        {
            // Log the exception and return a proper error message
            return StatusCode(500, new { message = "An error occurred while creating the statistika", error = ex.Message });
        }
    }

    [Route("api/actions")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStatistika(Guid id, [FromBody] Statistika model)
    {
        var existingStatistika = await _context.Statistikas.FindAsync(id);
        if (existingStatistika == null) return NotFound();

        existingStatistika.UserId = model.UserId;
        existingStatistika.Lloji_I_Pajisjes = model.Lloji_I_Pajisjes;
        existingStatistika.Rajoni = model.Rajoni;
        existingStatistika.Aplikacioni = model.Aplikacioni;
        existingStatistika.DataOra = model.DataOra;

        _context.Statistikas.Update(existingStatistika);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Statistika updated successfully." });
    }

    [Route("api/actions")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStatistika(Guid id)
    {
        var statistika = await _context.Statistikas.FindAsync(id);
        if (statistika == null) return NotFound();

        _context.Statistikas.Remove(statistika);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Statistika deleted successfully." });
    }

    [Route("api/reports")]
    [HttpGet]
    public IActionResult GetReportData(
    [FromQuery] string? rajoni = null,
    [FromQuery] string? aplikacioni = null,
    [FromQuery] string? llojiIPajisjes = null,
    [FromQuery] DateTime? startDate = null,
    [FromQuery] DateTime? endDate = null)
    {
        // Query the database and apply filters
        var statistikas = _context.Statistikas.AsQueryable();

        if (!string.IsNullOrEmpty(rajoni))
            statistikas = statistikas.Where(s => s.Rajoni.Contains(rajoni));

        if (!string.IsNullOrEmpty(aplikacioni))
            statistikas = statistikas.Where(s => s.Aplikacioni.Contains(aplikacioni));

        if (!string.IsNullOrEmpty(llojiIPajisjes))
            statistikas = statistikas.Where(s => s.Lloji_I_Pajisjes.Contains(llojiIPajisjes));

        if (startDate.HasValue)
            statistikas = statistikas.Where(s => s.DataOra >= startDate.Value);

        if (endDate.HasValue)
            statistikas = statistikas.Where(s => s.DataOra <= endDate.Value);

        // Grouping by Rajoni (Region)
        var groupedByRegion = statistikas
            .GroupBy(s => s.Rajoni)
            .Select(g => new ChartData
            {
                Label = g.Key,
                Count = g.Count()
            }).ToList();

        // Grouping by Aplikacioni (Application)
        var groupedByApplication = statistikas
            .GroupBy(s => s.Aplikacioni)
            .Select(g => new ChartData
            {
                Label = g.Key,
                Count = g.Count()
            }).ToList();

        return Ok(new
        {
            GroupedByRegion = groupedByRegion,
            GroupedByApplication = groupedByApplication
        });
    }
}
