using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using web_app_domain;
using web_app_performance.Controllers;
using web_app_repository;
using Xunit;

namespace Test
{
    public class ProdutoControllerTest
    {
        private readonly Mock<IProdutoRepository> _produtoRepositoryMock;
        private readonly ProdutoController _controller;

        public ProdutoControllerTest()
        {
            _produtoRepositoryMock = new Mock<IProdutoRepository>();
            _controller = new ProdutoController(_produtoRepositoryMock.Object);
        }

        [Fact]
        public async Task GetProduto_ShouldReturnOk_WithProdutos()
        {
            // Arrange
            var produtos = new List<Produto>
            {
                new Produto { Id = 1, Nome = "Produto 1", Preco = 10.0M },
                new Produto { Id = 2, Nome = "Produto 2", Preco = 20.0M }
            };
            _produtoRepositoryMock.Setup(repo => repo.ListarProdutos()).ReturnsAsync(produtos);

            // Act
            var result = await _controller.GetProduto();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(JsonConvert.SerializeObject(produtos), JsonConvert.SerializeObject(okResult.Value));
        }

        [Fact]
        public async Task GetProduto_ShouldReturnNotFound_WhenNoProdutos()
        {
            // Arrange
            _produtoRepositoryMock.Setup(u => u.ListarProdutos()).ReturnsAsync((IEnumerable<Produto>)null);

            // Act
            var result = await _controller.GetProduto();

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Post_ShouldSaveProduto_AndReturnOk()
        {
            // Arrange
            var produto = new Produto
            {
                Id = 1,
                Nome = "Produto Teste",
                Preco = 10.0M,
                QuantidadeEstoque = 50,
                DataCriacao = new DateTime(2024, 10, 1)
            };

            _produtoRepositoryMock.Setup(u => u.SalvarProduto(It.IsAny<Produto>()))
               .Returns(Task.CompletedTask);

            //Act 

            var result = await _controller.Post(produto);


            _produtoRepositoryMock.Verify(u => u.SalvarProduto(It.IsAny<Produto>()), Times.Once);

            Assert.IsType<OkObjectResult>(result);

        }


        [Fact]
        public async Task Put_ShouldUpdateProduto_AndReturnOk()
        {
            // Arrange
            var produto = new Produto
            {
                Id = 1,
                Nome = "Produto Atualizado",
                Preco = 15.0M,
                QuantidadeEstoque = 100,
                DataCriacao = new DateTime(2024, 10, 1)
            };
            _produtoRepositoryMock.Setup(repo => repo.AtualizarProduto(It.IsAny<Produto>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Put(produto);

            // Assert
            _produtoRepositoryMock.Verify(repo => repo.AtualizarProduto(It.Is<Produto>(p =>
                p.Id == produto.Id &&
                p.Nome == produto.Nome &&
                p.Preco == produto.Preco &&
                p.QuantidadeEstoque == produto.QuantidadeEstoque &&
                p.DataCriacao == produto.DataCriacao
            )), Times.Once);

            Assert.IsType<OkResult>(result);
        }


        [Fact]
        public async Task Delete_ShouldRemoveProduto_AndReturnOk()
        {
            // Arrange
            int id = 1;
            _produtoRepositoryMock.Setup(repo => repo.RemoverProduto(id)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            _produtoRepositoryMock.Verify(repo => repo.RemoverProduto(id), Times.Once);
            Assert.IsType<OkResult>(result);
        }
    }
}
