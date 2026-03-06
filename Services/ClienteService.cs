using Microsoft.EntityFrameworkCore;
using PizzariaApi.Data;
using PizzariaApi.Models;

namespace PizzariaApi.Services;

public class ClienteService : IClienteService
{
    private readonly AppDbContext _context;
    public ClienteService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Cliente>> ListarTodos()
    {
        return await _context.Clientes.ToListAsync();
    }

    public async Task<Cliente?> BuscarPorId(int id)
    {
        return await _context.Clientes.FindAsync(id);
    }

    public async Task<Cliente> Criar(Cliente cliente)
    {
        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();
        return cliente;
    }
}