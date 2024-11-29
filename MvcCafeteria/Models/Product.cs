using System.ComponentModel.DataAnnotations;

namespace MvcCafeteria.Models;

public class Product
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int Quantity { get; set; }
    public required string Category { get; set; }
    public decimal Price { get; set; }
}