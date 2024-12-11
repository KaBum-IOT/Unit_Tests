using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using MvcCafeteria.Controllers;
using MvcCafeteria.Models;
using Microsoft.EntityFrameworkCore;
using Cafeteria.Data;

public class ProductControllerTests
{
    [Fact]
    public async Task Create_ValidProduct_RedirectsToIndex()
    {
        // **Arrange**: Configuração do ambiente de teste
        
        // Configuração de um DbContext para simular o banco de dados
        var options = new DbContextOptionsBuilder<CafeteriaContext>()
            .UseInMemoryDatabase("TestDatabase") // Usa um banco de dados em memória para os testes
            .Options;

        // Criação de um mock do contexto (DbContext)
        var mockContext = new Mock<CafeteriaContext>(options);

        // Criação de um mock para o DbSet do tipo Product
        var mockDbSet = new Mock<DbSet<Product>>();

        // Configuração do comportamento do contexto simulado para retornar o DbSet simulado
        mockContext.Setup(c => c.Set<Product>()).Returns(mockDbSet.Object);

        // Salvar mudanças no contexto
        mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

        // Instância do controller sendo testado, com o contexto simulado
        var controller = new ProductsController(mockContext.Object);

        // Criação de um produto válido para o teste
        var validProduct = new Product
        {
            Id = 1,
            Name = "Test Product",
            Quantity = 10,
            Category = "Test Category",
            Price = 100.0m
        };

        // **Act**: Execução do método sendo testado
        var result = await controller.Create(validProduct);

        // **Assert**: Verificação dos resultados esperados

        // Verifica se o resultado é um redirecionamento para uma ação
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);

        // Verifica se o redirecionamento é para a ação "Index"
        Assert.Equal("Index", redirectToActionResult.ActionName);
    }

    [Fact]
    public async Task Create_InvalidModelState_ReturnsViewWithProduct()
    {
        var options = new DbContextOptionsBuilder<CafeteriaContext>()
            .UseInMemoryDatabase("TestDatabase") // Usa banco em memória para teste
            .Options;

        var mockContext = new Mock<CafeteriaContext>(options);

        var mockDbSet = new Mock<DbSet<Product>>();

        mockContext.Setup(c => c.Set<Product>()).Returns(mockDbSet.Object);

        mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

        var controller = new ProductsController(mockContext.Object);

        // Criação de um produto inválido (campo Name é nulo)
        var invalidProduct = new Product
        {
            Id = 1,
            Name = null,
            Quantity = -10,
            Category = "Test Category",
            Price = 100.0m
        };

        var result = await controller.Create(invalidProduct);

        // Verifica se o resultado é do tipo ViewResult
        var viewResult = Assert.IsType<ViewResult>(result);

        // Verifica se o modelo retornado na view é o mesmo modelo inválido enviado
        Assert.Equal(invalidProduct, viewResult.Model);
    }

    [Theory]
    [InlineData(1, null, 5, "Toys", 100.0)]
    [InlineData(1, "Coffe", -10, "Test Category", 100.0)]
    [InlineData(1, "Coffe", 10, null, 100.0)]
    public async Task Create_InvalidModelState_ReturnsViewWithProduct_Theory(int id, string name, int quantity, string category, decimal price)
    {
        var options = new DbContextOptionsBuilder<CafeteriaContext>()
            .UseInMemoryDatabase("TestDatabase") // Usa banco em memória para teste
            .Options;

        var mockContext = new Mock<CafeteriaContext>(options);

        var mockDbSet = new Mock<DbSet<Product>>();

        mockContext.Setup(c => c.Set<Product>()).Returns(mockDbSet.Object);

        mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

        var controller = new ProductsController(mockContext.Object);

        var invalidProduct = new Product
        {
            Id = id,
            Name = name,
            Quantity = quantity,
            Category = category,
            Price = price
        };

        var result = await controller.Create(invalidProduct);


        // Verifica se o resultado é do tipo ViewResult
        var viewResult = Assert.IsType<ViewResult>(result);

        // Verifica se o modelo retornado na view é o mesmo modelo inválido enviado
        Assert.Equal(invalidProduct, viewResult.Model);
    }

    public static IEnumerable<object[]> ProductTestData =>
        new List<object[]>
        {
            new object[]
            {
                new Product
                {
                    Id = 1,
                    Name = null,
                    Quantity = 10,
                    Category = null,
                    Price = -100.0m
                }
            }
        };

    [Theory]
    [MemberData(nameof(ProductTestData))]
    public async Task Create_InvalidModelState_ReturnsViewWithProduct_Theory_MemberDaya(Product product)
    {

        var options = new DbContextOptionsBuilder<CafeteriaContext>()
            .UseInMemoryDatabase("TestDatabase") // Usa banco em memória para teste
            .Options;

        var mockContext = new Mock<CafeteriaContext>(options);

        var mockDbSet = new Mock<DbSet<Product>>();

        mockContext.Setup(c => c.Set<Product>()).Returns(mockDbSet.Object);

        mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

        var controller = new ProductsController(mockContext.Object);

        var invalidProduct = new Product
        {
            Id = product.Id,
            Name = product.Name,
            Quantity = product.Quantity,
            Category = product.Category,
            Price = product.Price
        };

        var result = await controller.Create(invalidProduct);

        // Verifica se o resultado é do tipo ViewResult
        var viewResult = Assert.IsType<ViewResult>(result);

        // Verifica se o modelo retornado na view é o mesmo modelo inválido enviado
        Assert.Equal(invalidProduct, viewResult.Model);
    }
}