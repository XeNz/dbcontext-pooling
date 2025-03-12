using System.ComponentModel.DataAnnotations;

namespace DbContextPooling;

public sealed class TestModel
{
    [Key]
    public int Id { get; set; }

    public DateTime Date { get; set; }
    public int CampaignId { get; set; }
}