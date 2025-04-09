using App.Model;
using Microsoft.EntityFrameworkCore;

namespace App.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Item> Items { get; set; }
    public DbSet<ApiKey> ApiKeys { get; set; }
}
