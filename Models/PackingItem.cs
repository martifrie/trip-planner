using System.ComponentModel.DataAnnotations;

namespace TripPlanner.Models;

public class PackingItem
{
    public int Id { get; set; }

    public int TripId { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(50)]
    public string Category { get; set; } = "General";

    public bool IsPacked { get; set; }

    public bool IsSuggested { get; set; }

    public Trip Trip { get; set; } = null!;
}