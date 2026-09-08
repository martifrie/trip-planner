using System.ComponentModel.DataAnnotations;

namespace TripPlanner.Models;

public class VisaWindow
{
    public int Id { get; set; }

    public int DestinationId { get; set; }

    [Range(1, 365)]
    public int MaximumStayDays { get; set; }

    public DateOnly WindowStartsOn { get; set; }

    public DateOnly WindowEndsOn { get; set; }

    public Destination Destination { get; set; } = null!;
}