namespace TripPlanner.DTOs;

// What we send back to the client for a Trip.
// Notice: no back-reference to Destinations' Trip property, so no circular loop.
public class TripDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public string? Notes { get; set; }
    public string TripType { get; set; } = string.Empty; // enum sent as string, easy for JS to consume
    public List<DestinationDto> Destinations { get; set; } = new();
}

// What the client sends us to CREATE a trip.
// No Id (server assigns it), no Destinations (added separately via their own endpoint).
public class CreateTripDto
{
    public string Name { get; set; } = string.Empty;
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public string? Notes { get; set; }
    public string TripType { get; set; } = "Workation";
}