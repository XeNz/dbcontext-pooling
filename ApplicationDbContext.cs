using Microsoft.EntityFrameworkCore;

namespace DbContextPooling;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<TestModel> TestModels { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TestModel>()
            .HasIndex(t => new { t.CampaignId, t.Date })
            .HasDatabaseName("IX_TestModel_CampaignId_Date");
    }
}