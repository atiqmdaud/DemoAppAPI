using App.Data;
using App.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

namespace App.Endpoints;

public static class ApiKeyEndpoints
{
    public static void MapApiKeyEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/apikeys/generate",
            async (AppDbContext db, string user) =>
            {
                var apiKey = new ApiKey
                {
                    Key = Guid.NewGuid().ToString(),
                    User = user,
                    RemainingLimit = 1000, // Set initial request limit
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true,
                };
                db.ApiKeys.Add(apiKey);
                await db.SaveChangesAsync();
                return Results.Created($"/apikeys/{apiKey.Id}", apiKey);
            }
        );
    }
}
