using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PropertyService.Data;
using PropertyService.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PropertyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Add authentication
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = builder.Configuration["IdentityService:Authority"]; // URL of IdentityService
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false // Not validating audience in this example
        };
    });

// Add authorization
builder.Services.AddAuthorization();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PropertyDbContext>();
    dbContext.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication(); // Enable JWT authentication
app.UseAuthorization();  // Enable authorization middleware

var group = app.MapGroup("/api/properties");

// Endpoints
group.MapGet("/", async (PropertyDbContext context) =>
    await context.Properties.ToListAsync());

group.MapGet("/{id}", async (PropertyDbContext context, Guid id) =>
    await context.Properties.FindAsync(id) is Property property ? Results.Ok(property) : Results.NotFound());

group.MapPost("/", async (PropertyDbContext context, Property property) =>
{
    context.Properties.Add(property);
    await context.SaveChangesAsync();
    return Results.Created($"/api/properties/{property.Id}", property);
});

group.MapPut("/{id}", async (PropertyDbContext context, Guid id, Property updatedProperty) =>
{
    var property = await context.Properties.FindAsync(id);
    if (property is null) return Results.NotFound();

    property.Title = updatedProperty.Title;
    property.Description = updatedProperty.Description;
    property.Price = updatedProperty.Price;

    await context.SaveChangesAsync();
    return Results.NoContent();
});

group.MapDelete("/{id}", async (PropertyDbContext context, Guid id) =>
{
    var property = await context.Properties.FindAsync(id);
    if (property is null) return Results.NotFound();

    context.Properties.Remove(property);
    await context.SaveChangesAsync();

    return Results.NoContent();
});

app.Run();