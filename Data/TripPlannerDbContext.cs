using Microsoft.EntityFrameworkCore;
using TripPlanner.Models;

namespace TripPlanner.Data;

public class TripPlannerDbContext(DbContextOptions<TripPlannerDbContext> options)
    : DbContext(options)
{
    public DbSet<Trip> Trips => Set<Trip>();
    
    public DbSet<Destination> Destinations => Set<Destination>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Destination>(entity =>
        {
            entity.Property(destination => destination.PlannedBudget)
                .HasPrecision(12, 2);

            entity.HasOne(destination => destination.Trip)
                .WithMany(trip => trip.Destinations)
                .HasForeignKey(destination => destination.TripId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(destination => destination.VisaWindow)
                .WithOne(visaWindow => visaWindow.Destination)
                .HasForeignKey<VisaWindow>(visaWindow => visaWindow.DestinationId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.Property(destination => destination.ExpectedWeather)
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasDefaultValue(ExpectedWeather.Unknown);
            
            entity.Property(destination => destination.ActualSpend)
                .HasPrecision(12, 2)
                .HasDefaultValue(0m);
        });
        
        modelBuilder.Entity<Trip>(entity =>
        {
            entity.Property(trip => trip.TripType)
                .HasConversion<string>()
                .HasMaxLength(30)
                .HasDefaultValue(TripType.Workation);

            entity.HasMany(trip => trip.PackingItems)
                .WithOne(item => item.Trip)
                .HasForeignKey(item => item.TripId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
    
    public DbSet<VisaWindow> VisaWindows => Set<VisaWindow>();
    
    public DbSet<PackingItem> PackingItems => Set<PackingItem>();
}