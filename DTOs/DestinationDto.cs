namespace TripPlanner.DTOs;

public class DestinationDto
{
    public int Id { get; set; }
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public DateOnly ArrivalDate { get; set; }
    public DateOnly DepartureDate { get; set; }
    public decimal PlannedBudget { get; set; }
    public decimal ActualSpend { get; set; }
    public string CurrencyCode { get; set; } = "EUR";
    public string ExpectedWeather { get; set; } = string.Empty;
    // Notice: NO Trip property here — this is what breaks the circular reference
}

public class CreateDestinationDto
{
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public DateOnly ArrivalDate { get; set; }
    public DateOnly DepartureDate { get; set; }
    public decimal PlannedBudget { get; set; }
    public decimal ActualSpend { get; set; }
    public string CurrencyCode { get; set; } = "EUR";
    public string ExpectedWeather { get; set; } = "Unknown";
}
