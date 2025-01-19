using ActionTrackingService.Models;
using ActionTrackingService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add MongoDbService
builder.Services.AddSingleton<MongoDbService>();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

var group = app.MapGroup("/api/actions");

// Endpoints
group.MapPost("/", async (MongoDbService mongoDb, ActionLog actionLog) =>
{
    actionLog.CreatedAt = DateTime.UtcNow;
    await mongoDb.AddActionLog(actionLog);
    return Results.Created($"/api/actions/{actionLog.Id}", actionLog);
});

app.Run();