using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TripPlanner.Data;
using TripPlanner.DTOs;
using TripPlanner.Models;

namespace TripPlanner.Controllers;

[ApiController]
[Route("api/trips/{tripId}/destinations")]   // nested route — destinations always belong to a trip
public class DestinationsController(TripPlannerDbContext context) : ControllerBase
{
    // GET /api/trips/5/destinations
    [HttpGet]
    public async Task<ActionResult<List<DestinationDto>>> GetDestinations(int tripId)
    {
        var destinations = await context.Destinations
            .Where(d => d.TripId == tripId)      // like a WHERE clause / JPQL filter
            .Select(d => new DestinationDto
            {
                Id = d.Id,
                Country = d.Country,
                City = d.City,
                ArrivalDate = d.ArrivalDate,
                DepartureDate = d.DepartureDate,
                PlannedBudget = d.PlannedBudget,
                ActualSpend = d.ActualSpend,
                CurrencyCode = d.CurrencyCode,
                ExpectedWeather = d.ExpectedWeather.ToString()
            })
            .ToListAsync();

        return Ok(destinations);
    }

    // GET /api/trips/5/destinations/3
    [HttpGet("{id}")]
    public async Task<ActionResult<DestinationDto>> GetDestination(int tripId, int id)
    {
        var destination = await context.Destinations
            .FirstOrDefaultAsync(d => d.Id == id && d.TripId == tripId);

        if (destination is null) return NotFound();

        return Ok(new DestinationDto
        {
            Id = destination.Id,
            Country = destination.Country,
            City = destination.City,
            ArrivalDate = destination.ArrivalDate,
            DepartureDate = destination.DepartureDate,
            PlannedBudget = destination.PlannedBudget,
            ActualSpend = destination.ActualSpend,
            CurrencyCode = destination.CurrencyCode,
            ExpectedWeather = destination.ExpectedWeather.ToString()
        });
    }

    // POST /api/trips/5/destinations
    [HttpPost]
    public async Task<ActionResult<DestinationDto>> CreateDestination(int tripId, CreateDestinationDto dto)
    {
        // Confirm the parent trip actually exists before attaching a destination to it —
        // otherwise you'd silently create an orphaned row with a dangling foreign key.
        var tripExists = await context.Trips.AnyAsync(t => t.Id == tripId);
        if (!tripExists) return NotFound($"Trip {tripId} not found.");

        var destination = new Destination
        {
            TripId = tripId,                     // comes from the route, not the client body
            Country = dto.Country,
            City = dto.City,
            ArrivalDate = dto.ArrivalDate,
            DepartureDate = dto.DepartureDate,
            PlannedBudget = dto.PlannedBudget,
            ActualSpend = dto.ActualSpend,
            CurrencyCode = dto.CurrencyCode,
            ExpectedWeather = Enum.Parse<ExpectedWeather>(dto.ExpectedWeather)
        };

        context.Destinations.Add(destination);   // correct DbSet this time
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetDestination),
            new { tripId, id = destination.Id },
            new DestinationDto
            {
                Id = destination.Id,
                Country = destination.Country,
                City = destination.City,
                ArrivalDate = destination.ArrivalDate,
                DepartureDate = destination.DepartureDate,
                PlannedBudget = destination.PlannedBudget,
                ActualSpend = destination.ActualSpend,
                CurrencyCode = destination.CurrencyCode,
                ExpectedWeather = destination.ExpectedWeather.ToString()
            });
    }

    // DELETE /api/trips/5/destinations/3
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDestination(int tripId, int id)
    {
        var destination = await context.Destinations
            .FirstOrDefaultAsync(d => d.Id == id && d.TripId == tripId);

        if (destination is null) return NotFound();

        context.Destinations.Remove(destination);   // cascade removes its VisaWindow too
        await context.SaveChangesAsync();

        return NoContent();
    }
}