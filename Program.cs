using App.Data;
using App.Endpoints;
using App.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

var app = builder.Build();

app.UseMiddleware<ApiKeyMiddleware>();

ItemEndpoints.MapItemEndpoints(app);
ApiKeyEndpoints.MapApiKeyEndpoints(app);

app.Run();
