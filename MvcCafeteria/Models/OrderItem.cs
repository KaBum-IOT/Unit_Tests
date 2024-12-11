// Modelo que serve como uma entidade-relacionamento entre Order e Product

namespace MvcCafeteria.Models;

public class OrderItem
{
    public int Id { get; set; }
    public int IdProduct { get; set; }
    public int IdOrder { get; set; }
    public int Quantity { get; set; }
}