using MvcCafeteria.Models;

// Passa os parâmetros necessários para a view 'Details' dos pedidos (Order)
public class OrderDetailsViewModel
{
    public required Order Order { get; set; }
    public required Dictionary<Product, int> Products { get; set; }
}
