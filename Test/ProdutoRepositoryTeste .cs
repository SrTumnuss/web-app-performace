using Moq;
using web_app_domain;
using web_app_repository;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Test
{
    public class ProdutoRepositoryTest
    {
        [Fact]
        public async Task ListarProdutos_ShouldReturnListOfProdutos()
        {
            var produtos = new List<Produto>
            {
                new Produto
                {
                    Id = 1,
                    Nome = "Produto A",
                    Preco = 100.0M,
                    QuantidadeEstoque = 10
                },
                new Produto
                {
                    Id = 2,
                    Nome = "Produto B",
                    Preco = 200.0M,
                    QuantidadeEstoque = 5
                }
            };
            var produtoRepositoryMock = new Mock<IProdutoRepository>();
            produtoRepositoryMock.Setup(p => p.ListarProdutos()).ReturnsAsync(produtos);
            var produtoRepository = produtoRepositoryMock.Object;

            var result = await produtoRepository.ListarProdutos();

            Assert.Equal(produtos, result);
        }

        [Fact]
        public async Task SalvarProduto_ShouldCallSalvarProdutoOnce()
        {
            var produto = new Produto
            {
                Id = 1,
                Nome = "Produto Teste",
                Preco = 50.0M,
                QuantidadeEstoque = 20
            };
            var produtoRepositoryMock = new Mock<IProdutoRepository>();
            produtoRepositoryMock
                .Setup(p => p.SalvarProduto(It.IsAny<Produto>()))
                .Returns(Task.CompletedTask);
            var produtoRepository = produtoRepositoryMock.Object;

            await produtoRepository.SalvarProduto(produto);

            produtoRepositoryMock
                .Verify(p => p.SalvarProduto(It.IsAny<Produto>()), Times.Once);
        }

        [Fact]
        public async Task AtualizarProduto_ShouldCallAtualizarProdutoOnce()
        {
            var produto = new Produto
            {
                Id = 1,
                Nome = "Produto Atualizado",
                Preco = 15.0M,
                QuantidadeEstoque = 100,
                DataCriacao = new DateTime(2024, 10, 1)
            };

            var produtoRepositoryMock = new Mock<IProdutoRepository>();
            produtoRepositoryMock.Setup(p => p.AtualizarProduto(It.IsAny<Produto>()))
                .Returns(Task.CompletedTask);
            var produtoRepository = produtoRepositoryMock.Object;

            await produtoRepository.AtualizarProduto(produto);

            produtoRepositoryMock.Verify(p => p.AtualizarProduto(It.IsAny<Produto>()), Times.Once);
        }

        [Fact]
        public async Task RemoverProduto_ShouldCallRemoverProdutoOnce()
        {
            int id = 1;
            var produtoRepositoryMock = new Mock<IProdutoRepository>();
            produtoRepositoryMock.Setup(p => p.RemoverProduto(It.IsAny<int>()))
                .Returns(Task.CompletedTask);
            var produtoRepository = produtoRepositoryMock.Object;

            await produtoRepository.RemoverProduto(id);

            produtoRepositoryMock.Verify(p => p.RemoverProduto(It.IsAny<int>()), Times.Once);
        }
    }
}
