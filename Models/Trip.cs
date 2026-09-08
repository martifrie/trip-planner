using System.ComponentModel.DataAnnotations;

namespace TripPlanner.Models;

public class Trip
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }
    
    public ICollection<Destination> Destinations { get; set; } = new List<Destination>();
    
    public TripType TripType { get; set; } = TripType.Workation;

    public ICollection<PackingItem> PackingItems { get; set; } = new List<PackingItem>();
}