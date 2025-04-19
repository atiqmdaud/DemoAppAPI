using System.IO;

namespace App.Middleware;

public class LoggingMiddleware2
{
    private readonly RequestDelegate _next;

    public LoggingMiddleware2(RequestDelegate next)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context == null)
            throw new ArgumentNullException(nameof(context));

        // Log incoming request details
        LogRequestDetails(context);

        // Capture the original response body stream
        var originalBodyStream = context.Response.Body;

        await using var memoryStream = new MemoryStream();
        context.Response.Body = memoryStream;

        try
        {
            // Process the request pipeline
            await _next(context);

            // Log outgoing response details
            await LogResponseDetails(context, memoryStream);
        }
        finally
        {
            // Reset the response body to the original stream
            context.Response.Body = originalBodyStream;

            // Copy the content of MemoryStream back to the original stream
            memoryStream.Seek(0, SeekOrigin.Begin);
            await memoryStream.CopyToAsync(originalBodyStream);
        }
    }

    private void LogRequestDetails(HttpContext context)
    {
        Console.WriteLine("=== Incoming Request ===");
        Console.WriteLine($"Method: {context.Request.Method}");
        Console.WriteLine($"Path: {context.Request.Path}");

        foreach (var header in context.Request.Headers)
        {
            Console.WriteLine($"Header: {header.Key} = {header.Value}");
        }
        Console.WriteLine("========================\n");
    }

    private async Task LogResponseDetails(HttpContext context, MemoryStream memoryStream)
    {
        memoryStream.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(memoryStream).ReadToEndAsync();

        Console.WriteLine("=== Outgoing Response ===");
        Console.WriteLine($"Status Code: {context.Response.StatusCode}");
        Console.WriteLine($"Response Body: {responseBody}");
        Console.WriteLine("========================\n");
    }
}
