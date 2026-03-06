using PizzariaApi.Models;

namespace PizzariaApi.Services;

public interface IClienteService
{
    Task<IEnumerable<Cliente>> ListarTodos();
    Task<Cliente?> BuscarPorId(int id);
    Task<Cliente> Criar(Cliente cliente);


}