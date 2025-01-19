using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NotificationService.Data;
using NotificationService.Models;
using NotificationService.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<NotificationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost:6379"));
builder.Services.AddTransient<NotificationPublisher>();

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

// Migrate Database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();
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

var group = app.MapGroup("/api/notifications");

// Endpoints
group.MapPost("/", async (NotificationDbContext context, NotificationPublisher publisher, Notification notification) =>
{
    context.Notifications.Add(notification);
    await context.SaveChangesAsync();
    await publisher.PublishNotification("notifications", $"New notification: {notification.Message}");
    return Results.Created($"/api/notifications/{notification.Id}", notification);
});

app.Run();