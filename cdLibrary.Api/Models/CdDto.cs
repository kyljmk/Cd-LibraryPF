namespace efCdCollection.Api.Models;

public class CdDto
{
    public string? Title { get; set; }
    public string? Artist { get; set; }
    public string? Description { get; set; }
    public string? Genre { get; set; }
    public DateTime PurchaseDate { get; set; }
}