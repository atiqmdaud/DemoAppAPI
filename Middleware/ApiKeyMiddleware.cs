using App.Data;
using App.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace App.Middleware;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;

    public ApiKeyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, AppDbContext dbContext)
    {
        if (!context.Request.Headers.TryGetValue("X-API-KEY", out var apiKey))
        {
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("API Key is missing");
            return;
        }

        var apiKeyRecord = await dbContext.ApiKeys.FirstOrDefaultAsync(k =>
            k.Key == apiKey.ToString() && k.IsActive
        );
        if (apiKeyRecord == null)
        {
            context.Response.StatusCode = 403; // Forbidden
            await context.Response.WriteAsync("Invalid or inactive API Key");
            return;
        }

        // Check remaining limit
        if (apiKeyRecord.RemainingLimit <= 960)
        {
            context.Response.StatusCode = 429; // Too Many Requests
            await context.Response.WriteAsync("API limit exceeded for this key");
            return;
        }

        // Decrement API limit
        apiKeyRecord.RemainingLimit--;
        await dbContext.SaveChangesAsync();

        // Pass API key info to context for endpoints
        context.Items["ApiKey"] = apiKeyRecord;

        await _next(context);
    }
}
