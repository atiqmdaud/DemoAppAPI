using App.Data;
using App.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

namespace App.Endpoints;

public static class ItemEndpoints
{
    public static void MapItemEndpoints(IEndpointRouteBuilder app)
    {
        // CREATE
        app.MapPost(
            "/items",
            async (AppDbContext db, Item item) =>
            {
                db.Items.Add(item);
                await db.SaveChangesAsync();
                return Results.Created($"/items/{item.Id}", item);
            }
        );

        // READ ALL
        app.MapGet("/items", async (AppDbContext db) => await db.Items.ToListAsync());

        // READ BY ID
        app.MapGet(
            "/items/{id}",
            async (AppDbContext db, int id) =>
                await db.Items.FindAsync(id) is Item item ? Results.Ok(item) : Results.NotFound()
        );

        // UPDATE
        app.MapPut(
            "/items/{id}",
            async (AppDbContext db, int id, Item inputItem) =>
            {
                var item = await db.Items.FindAsync(id);
                if (item is null)
                    return Results.NotFound();

                item.Name = inputItem.Name;
                item.Description = inputItem.Description;
                await db.SaveChangesAsync();

                return Results.NoContent();
            }
        );

        // DELETE
        app.MapDelete(
            "/items/{id}",
            async (AppDbContext db, int id) =>
            {
                var item = await db.Items.FindAsync(id);
                if (item is null)
                    return Results.NotFound();

                db.Items.Remove(item);
                await db.SaveChangesAsync();

                return Results.NoContent();
            }
        );
    }
}
