using System.IO;

namespace App.Middleware;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;

    public LoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Log the incoming request
        Console.WriteLine($"Incoming Request: {context.Request.Method} {context.Request.Path}");

        // Log headers (optional)
        foreach (var header in context.Request.Headers)
        {
            Console.WriteLine($"Header: {header.Key} = {header.Value}");
        }

        // Capture the response before it's sent back to the client
        var originalBodyStream = context.Response.Body;

        using var memoryStream = new MemoryStream();
        context.Response.Body = memoryStream;

        // Process the request pipeline
        await _next(context);

        // Log the response
        memoryStream.Seek(0, SeekOrigin.Begin);
        var responseBody = new StreamReader(memoryStream).ReadToEnd();
        Console.WriteLine($"Outgoing Response: {context.Response.StatusCode}");
        Console.WriteLine($"Response Body: {responseBody}");

        // Reset the response body
        memoryStream.Seek(0, SeekOrigin.Begin);
        await memoryStream.CopyToAsync(originalBodyStream);
        context.Response.Body = originalBodyStream;
    }
}
