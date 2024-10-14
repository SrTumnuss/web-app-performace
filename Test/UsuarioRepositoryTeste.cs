using Moq;
using web_app_domain;
using web_app_repository;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Test
{
    public class UsuarioRepositoryTest
    {
        [Fact]
        public async Task ListarUsuarios_ShouldReturnListOfUsuarios()
        {
            var usuarios = new List<Usuario>
            {
                new Usuario
                {
                    Email = "xxx@gmail.com",
                    Id = 1,
                    Nome = "Thiago Xavier"
                },
                new Usuario
                {
                    Email = "usuario2@gmail.com",
                    Id = 2,
                    Nome = "Ana"
                }
            };

            var userRepositoryMock = new Mock<IUsuarioRepository>();
            userRepositoryMock.Setup(u => u.ListarUsuarios()).ReturnsAsync(usuarios);
            var userRepository = userRepositoryMock.Object;

            var result = await userRepository.ListarUsuarios();

            Assert.Equal(usuarios, result);
        }

        [Fact]
        public async Task SalvarUsuario_ShouldCallSalvarUsuarioOnce()
        {
            var usuario = new Usuario
            {
                Id = 1,
                Email = "thiago@fiap.com",
                Nome = "Thiago Xavier"
            };

            var userRepositoryMock = new Mock<IUsuarioRepository>();
            userRepositoryMock.Setup(u => u.SalvarUsuario(It.IsAny<Usuario>()))
                .Returns(Task.CompletedTask);
            var userRepository = userRepositoryMock.Object;

            await userRepository.SalvarUsuario(usuario);

            userRepositoryMock.Verify(u => u.SalvarUsuario(It.IsAny<Usuario>()), Times.Once);
        }

        [Fact]
        public async Task AtualizarUsuario_ShouldCallAtualizarUsuarioOnce()
        {
            var usuario = new Usuario
            {
                Id = 1,
                Email = "thiago@fiap.com",
                Nome = "Thiago Xavier"
            };

            var userRepositoryMock = new Mock<IUsuarioRepository>();
            userRepositoryMock.Setup(u => u.AtualizarUsuario(It.IsAny<Usuario>()))
                .Returns(Task.CompletedTask);
            var userRepository = userRepositoryMock.Object;

            await userRepository.AtualizarUsuario(usuario);

            userRepositoryMock.Verify(u => u.AtualizarUsuario(It.IsAny<Usuario>()), Times.Once);
        }

        [Fact]
        public async Task RemoverUsuario_ShouldCallRemoverUsuarioOnce()
        {
            int id = 1;
            var userRepositoryMock = new Mock<IUsuarioRepository>();
            userRepositoryMock.Setup(u => u.RemoverUsuario(It.IsAny<int>()))
                .Returns(Task.CompletedTask);
            var userRepository = userRepositoryMock.Object;

            await userRepository.RemoverUsuario(id);

            userRepositoryMock.Verify(u => u.RemoverUsuario(It.IsAny<int>()), Times.Once);
        }
    }
}
