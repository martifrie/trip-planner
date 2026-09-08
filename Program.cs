using Microsoft.EntityFrameworkCore;
using TripPlanner.Data;
using TripPlanner.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Scoped DbContext, one instance per HTTP request
builder.Services.AddDbContext<TripPlannerDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("TripPlanner")));

builder.Services.AddSingleton<PackingSuggestionService>();

// CORS policy allowing the React dev server to call this API.
// TODO: restrict origin to your actual deployed frontend URL before going to production.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCors("AllowReactApp"); // must come before MapControllers
app.MapControllers();

app.Run();