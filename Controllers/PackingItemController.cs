using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TripPlanner.Data;
using TripPlanner.DTOs;
using TripPlanner.Models;

namespace TripPlanner.Controllers;

[ApiController]
[Route("api/trips/{tripId}/packingitems")]
public class PackingItemsController(TripPlannerDbContext context) : ControllerBase
{
    // GET /api/trips/5/packingitems
    [HttpGet]
    public async Task<ActionResult<List<PackingItemDto>>> GetPackingItems(int tripId)
    {
        var items = await context.PackingItems
            .Where(i => i.TripId == tripId)
            .Select(i => new PackingItemDto
            {
                Id = i.Id,
                Name = i.Name,
                Category = i.Category,
                IsPacked = i.IsPacked,
                IsSuggested = i.IsSuggested
            })
            .ToListAsync();

        return Ok(items);
    }

    // POST /api/trips/5/packingitems
    [HttpPost]
    public async Task<ActionResult<PackingItemDto>> CreatePackingItem(int tripId, CreatePackingItemDto dto)
    {
        var tripExists = await context.Trips.AnyAsync(t => t.Id == tripId);
        if (!tripExists) return NotFound($"Trip {tripId} not found.");

        var item = new PackingItem
        {
            TripId = tripId,
            Name = dto.Name,
            Category = dto.Category,
            IsPacked = false,
            IsSuggested = false // manually added, not a system suggestion
        };

        context.PackingItems.Add(item);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPackingItems), new { tripId },
            new PackingItemDto
            {
                Id = item.Id,
                Name = item.Name,
                Category = item.Category,
                IsPacked = item.IsPacked,
                IsSuggested = item.IsSuggested
            });
    }

    // PATCH /api/trips/5/packingitems/3/toggle
    // A dedicated endpoint for the one thing users will do most: check/uncheck an item.
    // Simpler than a full PUT that requires resending every field just to flip one boolean.
    [HttpPatch("{id}/toggle")]
    public async Task<ActionResult<PackingItemDto>> TogglePacked(int tripId, int id)
    {
        var item = await context.PackingItems
            .FirstOrDefaultAsync(i => i.Id == id && i.TripId == tripId);

        if (item is null) return NotFound();

        item.IsPacked = !item.IsPacked;
        await context.SaveChangesAsync();

        return Ok(new PackingItemDto
        {
            Id = item.Id,
            Name = item.Name,
            Category = item.Category,
            IsPacked = item.IsPacked,
            IsSuggested = item.IsSuggested
        });
    }

    // DELETE /api/trips/5/packingitems/3
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePackingItem(int tripId, int id)
    {
        var item = await context.PackingItems
            .FirstOrDefaultAsync(i => i.Id == id && i.TripId == tripId);

        if (item is null) return NotFound();

        context.PackingItems.Remove(item);
        await context.SaveChangesAsync();

        return NoContent();
    }
}