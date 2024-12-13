# Sistema MVC Cafeteria Web

Este projeto consiste em um sistema de estoque de produtos e pedidos para uma cafeteria de alimentos e bebidas. Ele foi desenvolvido em *C#* com armazenamento de dados utilizando *SQLite*.

## Testes Unitários

Os testes unitários foram realizados na classe Teste, especificamente no método Create do *ProductsController*. Esse método processa um JSON contendo os dados de uma linha da tabela de produtos.  

Para os testes, utilizamos a biblioteca *xUnit, juntamente com o **Moq*, que permitiu recriar uma versão temporária do banco de dados e da tabela. Os testes foram implementados utilizando as abordagens Fact e Theory do xUnit.

### Como Executar os Testes

1. Navegue até a pasta do projeto principal, chamada *MvcCafeteria*.  
2. Execute o seguinte comando no terminal:  
   bash
   dotnet test
   
### Testes Implementados

Os testes unitários realizados verificaram o comportamento do método Create em diferentes cenários:

- Dados válidos: O método deve redirecionar corretamente para a View do produto criado.
- Dados inválidos: O método deve retornar para a página inicial (Index) de produtos.

### Autores

- Guilherme Canarini Kaneda
- Gabriel Faria e Silva