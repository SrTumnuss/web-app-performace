using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using web_app_domain;
using web_app_performance.Controllers;
using web_app_repository;

namespace Test
{
    public class UsuarioControllerTest
    {
        private readonly Mock<IUsuarioRepository> _userRepositoryMock;
        private readonly UsuarioController _controller;

        public UsuarioControllerTest()
        {
            _userRepositoryMock = new Mock<IUsuarioRepository>();
            _controller = new UsuarioController(_userRepositoryMock.Object);


        }

        [Fact]
        public async Task Get_ListarUsariosOk()
        {
            var usuarios = new List<Usuario>()
            {

                new Usuario()
                {
                    Id = 1,
                    Nome = "LUCAS",
                    Email = "xxxx@gmail.com"
                }
            };
            _userRepositoryMock.Setup(r => r.ListarUsuarios()).ReturnsAsync(usuarios);


            var result = await _controller.GetUsuario();
            
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal(JsonConvert.SerializeObject(usuarios), JsonConvert.SerializeObject(okResult.Value));
        }


   

        [Fact]
        public async Task Get_ListarRetornaNotFound()
        {
            _userRepositoryMock.Setup(u => u.ListarUsuarios()).ReturnsAsync((IEnumerable<Usuario>)null);

            var result = await _controller.GetUsuario();

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Post_SalvarUsuario()
        {
            //arrange
            var usuario = new Usuario()
            {
                Id = 1,
                Email = "lucas@fiap.com",
                Nome = "Lucas"
            };
            _userRepositoryMock.Setup(u => u.SalvarUsuario(It.IsAny<Usuario>()))
                .Returns(Task.CompletedTask);

            //Act 

            var result = await _controller.Post(usuario);


            _userRepositoryMock.Verify(u => u.SalvarUsuario(It.IsAny<Usuario>()), Times.Once);

            Assert.IsType<OkObjectResult>(result);

        }

        [Fact]
        public async Task Put_ShouldUpdateUsuario_AndReturnOk()
        {
            var usuario = new Usuario
            {
                Id = 1,
                Nome = "Thiago Atualizado",
                Email = "thiago_atualizado@fiap.com"
            };
            _userRepositoryMock.Setup(repo => repo.AtualizarUsuario(It.IsAny<Usuario>())).Returns(Task.CompletedTask);

            var result = await _controller.Put(usuario);

            _userRepositoryMock.Verify(repo => repo.AtualizarUsuario(It.Is<Usuario>(u =>
                u.Id == usuario.Id &&
                u.Nome == usuario.Nome &&
                u.Email == usuario.Email
            )), Times.Once);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Delete_ShouldRemoveUsuario_AndReturnOk()
        {
            int id = 1;
            _userRepositoryMock.Setup(repo => repo.RemoverUsuario(id)).Returns(Task.CompletedTask);

            var result = await _controller.Delete(id);

            _userRepositoryMock.Verify(repo => repo.RemoverUsuario(id), Times.Once);
            Assert.IsType<OkResult>(result);
        }

    }
} 

