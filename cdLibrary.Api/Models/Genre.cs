using System.ComponentModel.DataAnnotations;

namespace cdLibrary.Api.Models;

public class Genre
{
    public int Id { get; set; }
    public string? Name { get; set; }
}