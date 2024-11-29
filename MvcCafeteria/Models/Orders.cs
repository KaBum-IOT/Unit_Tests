using System.ComponentModel.DataAnnotations;

namespace MvcCafeteria.Models;

public class Orders
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public decimal TotalPrice { get; set; }
}