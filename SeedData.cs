using Microsoft.EntityFrameworkCore;

namespace DbContextPooling;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        await using var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

        if (await context.TestModels.AnyAsync())
        {
            return;
        }

        const int batchSize = 1000;
        const int totalRecords = 150000;
        var random = new Random();

        for (var i = 0; i < totalRecords; i += batchSize)
        {
            var records = new List<TestModel>();

            for (var j = 0; j < batchSize && (i + j) < totalRecords; j++)
            {
                var daysAgo = random.Next(0, 30);
                var date = DateTime.UtcNow.Date.AddDays(-daysAgo);

                var campaignId = random.Next(1, 300);

                records.Add(new TestModel
                {
                    Date = date.AddHours(random.Next(0, 24)).AddMinutes(random.Next(0, 60)),
                    CampaignId = campaignId
                });
            }

            await context.TestModels.AddRangeAsync(records);
            await context.SaveChangesAsync();

            Console.WriteLine($"Added {i + records.Count} records out of {totalRecords}");
        }
    }
}