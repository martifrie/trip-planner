using System.ComponentModel.DataAnnotations;

namespace TripPlanner.Models;

public class Destination
{
    public int Id { get; set; }

    public int TripId { get; set; } // Foreign key

    [Required, StringLength(100)]
    public string Country { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string City { get; set; } = string.Empty;

    public DateOnly ArrivalDate { get; set; }

    public DateOnly DepartureDate { get; set; }

    public decimal PlannedBudget { get; set; }
    
    public decimal ActualSpend { get; set; }

    [StringLength(3)]
    public string CurrencyCode { get; set; } = "EUR";

    public Trip Trip { get; set; } = null!;
    
    public VisaWindow? VisaWindow { get; set; }
    
    public ExpectedWeather ExpectedWeather { get; set; } = ExpectedWeather.Unknown;
}