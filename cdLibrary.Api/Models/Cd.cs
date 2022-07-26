using System.ComponentModel.DataAnnotations;

namespace cdLibrary.Api.Models;

public class Cd
{
    [Key]
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Artist { get; set; }
    public string? Description { get; set; }
    public Genre? Genre { get; set; }
    public DateTime PurchaseDate { get; set; }
}