using FinanceStock.Models;
using Microsoft.EntityFrameworkCore;
namespace FinanceStock;

public class StockDbContext : DbContext
{
    public DbSet<StockData> StockData { get; set; }
    public DbSet<GoodStock> GoodStocks { get; set; }

    public StockDbContext(DbContextOptions<StockDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StockData>()
            .HasKey(s => new { s.Symbol, s.Date }); // Composite key cho StockData

        modelBuilder.Entity<GoodStock>()
            .HasKey(g => g.Id); // Primary key cho GoodStock
    }
}