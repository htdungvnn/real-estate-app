using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UserService.Data;
using UserService.Models;

var builder = WebApplication.CreateBuilder(args);

// Add EF Core
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Swagger
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

// Migrate database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();
    dbContext.Database.Migrate();
}

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication(); // Enable JWT authentication
app.UseAuthorization();  // Enable authorization middleware

var group = app.MapGroup("/api/users");

// Endpoints
group.MapGet("/", async (UserDbContext context) =>
    await context.Users.ToListAsync());

group.MapGet("/{id}", async (UserDbContext context, Guid id) =>
    await context.Users.FindAsync(id) is User user ? Results.Ok(user) : Results.NotFound());

group.MapPost("/", async (UserDbContext context, User user) =>
{
    context.Users.Add(user);
    await context.SaveChangesAsync();
    return Results.Created($"/api/users/{user.Id}", user);
});

group.MapPut("/{id}", async (UserDbContext context, Guid id, User updatedUser) =>
{
    var user = await context.Users.FindAsync(id);
    if (user is null) return Results.NotFound();

    user.FirstName = updatedUser.FirstName;
    user.LastName = updatedUser.LastName;
    user.Email = updatedUser.Email;

    await context.SaveChangesAsync();
    return Results.NoContent();
});

group.MapDelete("/{id}", async (UserDbContext context, Guid id) =>
{
    var user = await context.Users.FindAsync(id);
    if (user is null) return Results.NotFound();

    context.Users.Remove(user);
    await context.SaveChangesAsync();

    return Results.NoContent();
});

app.Run();