using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
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
        
        // Configuração de um DbContext em memória para simular o banco de dados
        var options = new DbContextOptionsBuilder<CafeteriaContext>()
            .UseInMemoryDatabase("TestDatabase") // Usa um banco de dados em memória para os testes
            .Options;

        // Criação de um mock do contexto (DbContext)
        var mockContext = new Mock<CafeteriaContext>(options);

        // Criação de um mock para o DbSet do tipo Product
        var mockDbSet = new Mock<DbSet<Product>>();

        // Configuração do comportamento do contexto simulado para retornar o DbSet simulado
        mockContext.Setup(c => c.Set<Product>()).Returns(mockDbSet.Object);

        // Simulação do comportamento de salvar mudanças no contexto
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

        // Verifica se o método Add foi chamado uma vez para adicionar o produto
        mockContext.Verify(c => c.Add(It.IsAny<Product>()), Times.Once);

        // Verifica se SaveChangesAsync foi chamado uma vez para salvar no banco
        mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
    }

    [Fact]
    public async Task Create_InvalidModelState_ReturnsViewWithProduct()
    {
        // **Arrange**: Configuração do ambiente de teste

        // Configuração do DbContext em memória
        var options = new DbContextOptionsBuilder<CafeteriaContext>()
            .UseInMemoryDatabase("TestDatabase") // Usa banco em memória para teste
            .Options;

        // Criação de um mock do contexto (DbContext)
        var mockContext = new Mock<CafeteriaContext>(options);

        // Criação de um mock para o DbSet do tipo Product
        var mockDbSet = new Mock<DbSet<Product>>();

        // Configuração do comportamento do contexto para retornar o DbSet simulado
        mockContext.Setup(c => c.Set<Product>()).Returns(mockDbSet.Object);

        // Simulação do comportamento de salvar mudanças no contexto
        mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1);

        // Instância do controller sendo testado, com o contexto simulado
        var controller = new ProductsController(mockContext.Object);

        // Criação de um produto inválido (campo Name é nulo)
        var invalidProduct = new Product
        {
            Id = 1,
            Name = null, // ou string.Empty para simular um erro de validação
            Quantity = -10,
            Category = "Test Category",
            Price = 100.0m
        };

        // **Act**: Execução do método sendo testado
        var result = await controller.Create(invalidProduct);

        // **Assert**: Verificação dos resultados esperados

        // Verifica se o resultado é do tipo ViewResult
        var viewResult = Assert.IsType<ViewResult>(result);

        // Verifica se o modelo retornado na view é o mesmo modelo inválido enviado
        Assert.Equal(invalidProduct, viewResult.Model);

        // Verifica que o método Add nunca foi chamado (pois o modelo é inválido)
        mockContext.Verify(m => m.Add(It.IsAny<Product>()), Times.Never);

        // Verifica que SaveChangesAsync nunca foi chamado
        mockContext.Verify(m => m.SaveChangesAsync(default), Times.Never);
    }
}