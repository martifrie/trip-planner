namespace TripPlanner.DTOs;

public class PackingItemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public bool IsPacked { get; set; }
    public bool IsSuggested { get; set; }
}

// No TripId (comes from the route) or IsSuggested (server-side logic decides this,
// not something a user manually sets when adding an item themselves).
public class CreatePackingItemDto
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = "General";
}