using MySqlConnector;
using web_app_domain;
using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace web_app_repository
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly MySqlConnection mySqlConnection;
        public ProdutoRepository()
        {
            string connectionString = "Server=localhost;Database=sys;User=root;Password=123;";
            mySqlConnection = new MySqlConnection(connectionString);
        }

        public async Task<IEnumerable<Produto>> ListarProdutos()
        {
            await mySqlConnection.OpenAsync();
            string query = "select Id, Nome, Preco, QuantidadeEstoque, DataCriacao from produtos;";
            var produtos = await mySqlConnection.QueryAsync<Produto>(query);
            await mySqlConnection.CloseAsync();

            return produtos;
        }

        public async Task SalvarProduto(Produto produto)
        {
            await mySqlConnection.OpenAsync();
            string sql = @"insert into produtos(Nome, Preco, QuantidadeEstoque, DataCriacao) 
                            values(@Nome, @Preco, @QuantidadeEstoque, @DataCriacao);";
            await mySqlConnection.ExecuteAsync(sql, produto);
            await mySqlConnection.CloseAsync();
        }

        public async Task AtualizarProduto(Produto produto)
        {
            await mySqlConnection.OpenAsync();
            string sql = @"update produtos 
                            set Nome = @Nome, 
	                            Preco = @Preco, 
	                            QuantidadeEstoque = @QuantidadeEstoque, 
	                            DataCriacao = @DataCriacao
                            where Id = @Id;";

            await mySqlConnection.ExecuteAsync(sql, produto);
            await mySqlConnection.CloseAsync();
        }

        public async Task RemoverProduto(int id)
        {
            await mySqlConnection.OpenAsync();
            string sql = @"delete from produtos where Id = @id;";
            await mySqlConnection.ExecuteAsync(sql, new { id });
            await mySqlConnection.CloseAsync();
        }
    }
}
