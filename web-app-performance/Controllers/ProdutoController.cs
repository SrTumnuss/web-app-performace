using Dapper;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Newtonsoft.Json;
using StackExchange.Redis;
using web_app_domain;
using web_app_repository;
using RabbitMQ.Client;
using System.Text;

namespace web_app_performance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private static ConnectionMultiplexer redis;
        private readonly IProdutoRepository _repository;

        public ProdutoController(IProdutoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetProduto()
        {
            var produtos = await _repository.ListarProdutos();

            if (produtos == null)
                return NotFound();

            string produtosJson = JsonConvert.SerializeObject(produtos);
            return Ok(produtos);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Produto produto)
        {
            await _repository.SalvarProduto(produto);

            // Enviar mensagem de sucesso para o RabbitMQ
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "fila_teste",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                string mensagem = $"Produto {produto.Nome} criado com sucesso!";
                var body = Encoding.UTF8.GetBytes(mensagem);

                channel.BasicPublish(exchange: "",
                                     routingKey: "fila_teste",
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine($"[x] Enviado: {mensagem}");
            }

            return Ok(new { mensagem = "Criado com Sucesso!" });
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Produto produto)
        {
            await _repository.AtualizarProduto(produto);

            // Enviar mensagem de sucesso para o RabbitMQ
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "fila_teste",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                string mensagem = $"Produto {produto.Nome} atualizado com sucesso!";
                var body = Encoding.UTF8.GetBytes(mensagem);

                channel.BasicPublish(exchange: "",
                                     routingKey: "fila_teste",
                                     basicProperties: null,
                                     body: body);
                Console.WriteLine($"[x] Enviado: {mensagem}");
            }

            return Ok(new { mensagem = "Atualizado com Sucesso!" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.RemoverProduto(id);
            return Ok();
        }
    }
}
