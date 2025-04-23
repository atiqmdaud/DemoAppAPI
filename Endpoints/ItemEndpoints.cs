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
        app.MapPost("/items", CreateItem);

        // READ ALL
        app.MapGet("/items", GetItems);

        // READ BY ID
        app.MapGet("/items/{id}", GetItemById);

        // UPDATE
        app.MapPut("/items/{id}", UpdateItem);

        // DELETE
        app.MapDelete("/items/{id}", DeleteItem);
    }

    private static async Task<IResult> CreateItem(AppDbContext db, Item item)
    {
        try
        {
            await db.Items.AddAsync(item);
            await db.SaveChangesAsync();
            return Results.Created($"/items/{item.Id}", item);
        }
        catch (Exception ex)
        {
            // Log the exception (optional)
            return Results.Problem("An error occurred while creating the item.");
        }
    }

    private static async Task<IResult> GetItems(AppDbContext db)
    {
        try
        {
            var items = await db.Items.ToListAsync();
            return Results.Ok(items);
        }
        catch (Exception ex)
        {
            // Log the exception (optional)
            return Results.Problem("An error occurred while retrieving the items.");
        }
    }

    private static async Task<IResult> GetItemById(AppDbContext db, int id)
    {
        try
        {
            var item = await db.Items.FindAsync(id);
            if (item is null)
                return Results.NotFound();
            return Results.Ok(item);
        }
        catch (Exception ex)
        {
            // Log the exception (optional)
            return Results.Problem("An error occurred while retrieving the item.");
        }
    }

    private static async Task<IResult> UpdateItem(AppDbContext db, int id, Item inputItem)
    {
        try
        {
            var item = await db.Items.FindAsync(id);
            if (item is null)
                return Results.NotFound();

            item.Name = inputItem.Name;
            item.Description = inputItem.Description;

            await db.SaveChangesAsync();

            return Results.NoContent();
        }
        catch (Exception ex)
        {
            // Log the exception (optional)
            return Results.Problem("An error occurred while updating the item.");
        }
    }

    private static async Task<IResult> DeleteItem(AppDbContext db, int id)
    {
        try
        {
            var item = await db.Items.FindAsync(id);
            if (item is null)
                return Results.NotFound();

            db.Items.Remove(item);
            await db.SaveChangesAsync();

            return Results.NoContent();
        }
        catch (Exception ex)
        {
            // Log the exception (optional)
            return Results.Problem("An error occurred while deleting the item.");
        }
    }
}
