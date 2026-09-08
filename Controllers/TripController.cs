using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TripPlanner.Data;
using TripPlanner.DTOs;
using TripPlanner.Models;

namespace TripPlanner.Controllers;

[ApiController]                    // like @RestController — enables automatic model validation, binds JSON body automatically
[Route("api/[controller]")]        // like @RequestMapping("/api/trips") — [controller] becomes "Trips" (class name minus "Controller")
public class TripsController(TripPlannerDbContext context) : ControllerBase
{
    // GET /api/trips
    [HttpGet]                      // like @GetMapping
    public async Task<ActionResult<List<TripDto>>> GetTrips()
    {
        var trips = await context.Trips
            .Include(t => t.Destinations)   // like JOIN FETCH in JPQL — eager-loads Destinations
            .Select(t => new TripDto
            {
                Id = t.Id,
                Name = t.Name,
                StartDate = t.StartDate,
                EndDate = t.EndDate,
                Notes = t.Notes,
                TripType = t.TripType.ToString(),
                Destinations = t.Destinations.Select(d => new DestinationDto
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
                }).ToList()
            })
            .ToListAsync();

        return Ok(trips);          // like ResponseEntity.ok(trips)
    }

    // GET /api/trips/5
    [HttpGet("{id}")]              // like @GetMapping("/{id}")
    public async Task<ActionResult<TripDto>> GetTrip(int id)
    {
        var trip = await context.Trips
            .Include(t => t.Destinations)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (trip is null) return NotFound();   // like ResponseEntity.notFound().build()

        return Ok(new TripDto
        {
            Id = trip.Id,
            Name = trip.Name,
            StartDate = trip.StartDate,
            EndDate = trip.EndDate,
            Notes = trip.Notes,
            TripType = trip.TripType.ToString(),
            Destinations = trip.Destinations.Select(d => new DestinationDto
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
            }).ToList()
        });
    }

    // POST /api/trips
    [HttpPost]                     // like @PostMapping
    public async Task<ActionResult<TripDto>> CreateTrip(CreateTripDto dto)
    {
        // [ApiController] auto-validates [Required]/[StringLength] on the DTO
        // and returns 400 Bad Request automatically if invalid — like @Valid in Spring

        var trip = new Trip
        {
            Name = dto.Name,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Notes = dto.Notes,
            TripType = Enum.Parse<TripType>(dto.TripType)
        };

        context.Trips.Add(trip);
        await context.SaveChangesAsync();   // like repository.save() + transaction commit

        return CreatedAtAction(nameof(GetTrip), new { id = trip.Id },
            new TripDto { Id = trip.Id, Name = trip.Name, StartDate = trip.StartDate,
                          EndDate = trip.EndDate, Notes = trip.Notes, TripType = trip.TripType.ToString() });
        // like ResponseEntity.created(uri).body(dto) — 201 with a Location header
    }

    // DELETE /api/trips/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTrip(int id)
    {
        var trip = await context.Trips.FindAsync(id);
        if (trip is null) return NotFound();

        context.Trips.Remove(trip);         // cascade delete removes Destinations/PackingItems too
        await context.SaveChangesAsync();

        return NoContent();                 // like ResponseEntity.noContent().build() — 204
    }
}