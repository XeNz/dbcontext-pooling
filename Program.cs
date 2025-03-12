using DbContextPooling;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddPooledDbContextFactory<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/api/test",
        async (int campaignId, IDbContextFactory<ApplicationDbContext> contextFactory) =>
        {
            await using var context = await contextFactory.CreateDbContextAsync();
            var newRecord = new TestModel
            {
                CampaignId = campaignId,
                Date = DateTime.UtcNow
            };

            context.TestModels.Add(newRecord);
            await context.SaveChangesAsync();

            return Results.Ok(new { id = newRecord.Id, message = "Record added successfully" });
        })
    .WithName("AddRecord")
    .WithOpenApi();

app.MapGet("/api/test",
        async (int campaignId, IDbContextFactory<ApplicationDbContext> contextFactory) =>
        {
            await using var context = await contextFactory.CreateDbContextAsync();
            var today = DateTime.UtcNow.Date;
            var tomorrow = today.AddDays(1);

            var count = await context.TestModels
                .Where(t => t.CampaignId == campaignId && t.Date >= today && t.Date < tomorrow)
                .CountAsync();

            return Results.Ok(new { campaignId, date = today.ToString("d"), count });
        })
    .WithName("CountTodayRecords")
    .WithOpenApi();

app.MapGet("/api/test/seed",
        async (IServiceProvider serviceProvider) =>
        {
            await SeedData.Initialize(serviceProvider);
        })
    .WithName("SeedData")
    .WithOpenApi();

app.Run();