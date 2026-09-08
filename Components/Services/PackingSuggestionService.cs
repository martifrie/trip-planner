using TripPlanner.Models;

namespace TripPlanner.Services;

public class PackingSuggestionService
{
public IReadOnlyList<PackingSuggestion> GetSuggestions(
    TripType tripType,
    IEnumerable<ExpectedWeather> expectedWeather)
{
    var suggestions = GetTripTypeSuggestions(tripType).ToList();

    foreach (var weather in expectedWeather.Distinct())
    {
        suggestions.AddRange(GetWeatherSuggestions(weather));
    }

    return suggestions
        .DistinctBy(suggestion => suggestion.Name, StringComparer.OrdinalIgnoreCase)
        .ToList();
}

private static IEnumerable<PackingSuggestion> GetTripTypeSuggestions(TripType tripType)
{
    return tripType switch
    {
        TripType.Workation => new[]
        {
            new PackingSuggestion("Laptop", "Electronics"),
            new PackingSuggestion("Laptop charger", "Electronics"),
            new PackingSuggestion("Universal power adapter", "Electronics"),
            new PackingSuggestion("Noise-cancelling headphones", "Electronics")
        },

        TripType.Beach => new[]
        {
            new PackingSuggestion("Swimwear", "Clothing"),
            new PackingSuggestion("Sun hat", "Accessories")
        },

        TripType.Hiking => new[]
        {
            new PackingSuggestion("Trail shoes", "Footwear"),
            new PackingSuggestion("Reusable water bottle", "Equipment")
        },

        _ => new[]
        {
            new PackingSuggestion("Comfortable walking shoes", "Footwear"),
            new PackingSuggestion("Reusable water bottle", "General")
        }
    };
}

private static IEnumerable<PackingSuggestion> GetWeatherSuggestions(
    ExpectedWeather weather)
{
    return weather switch
    {
        ExpectedWeather.Hot => new[]
        {
            new PackingSuggestion("Sunscreen", "Toiletries"),
            new PackingSuggestion("Lightweight clothing", "Clothing")
        },

        ExpectedWeather.Cold => new[]
        {
            new PackingSuggestion("Warm jacket", "Outerwear"),
            new PackingSuggestion("Thermal base layer", "Clothing")
        },

        ExpectedWeather.Rainy => new[]
        {
            new PackingSuggestion("Waterproof jacket", "Outerwear"),
            new PackingSuggestion("Compact umbrella", "Accessories")
        },

        _ => Array.Empty<PackingSuggestion>()
    };
}
}

public record PackingSuggestion(string Name, string Category);