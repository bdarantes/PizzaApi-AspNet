using PizzariaApi.Models;

namespace PizzariaApi.Services;

public interface IProdutoService
{
    Task<IEnumerable<Produto>> ListarAtivos();
    Task<Produto?> BuscarPorId(int id);
    Task<Produto> Criar(Produto produto);
    Task Atualizar(int id, Produto produto);
    Task<bool> SoftDelete(int id);
}