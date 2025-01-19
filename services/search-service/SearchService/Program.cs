using Microsoft.EntityFrameworkCore;
using Nest;
using SearchService.Models;
using SearchService.Data;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddDbContext<SearchDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Elasticsearch
builder.Services.AddSingleton<IElasticClient>(provider =>
{
    var settings = new ConnectionSettings(new Uri(builder.Configuration["Elasticsearch:Uri"]))
        .DefaultIndex("properties");
    return new ElasticClient(settings);
});


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
    var dbContext = scope.ServiceProvider.GetRequiredService<SearchDbContext>();
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

var group = app.MapGroup("/api/search");

// Endpoints
group.MapGet("/", async (SearchDbContext context) =>
    await context.SearchMetadata.ToListAsync());

group.MapPost("/", async (IElasticClient client, SearchMetadata metadata) =>
{
    var response = await client.IndexDocumentAsync(metadata);
    return response.IsValid ? Results.Ok(response) : Results.BadRequest(response.DebugInformation);
});

app.Run();