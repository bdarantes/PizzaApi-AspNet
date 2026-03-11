using Microsoft.EntityFrameworkCore;
using PizzariaApi.Data;
using PizzariaApi.Models;
using PizzariaApi.Exceptions;

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
        return await _context.Clientes.AsNoTracking().ToListAsync();
    }

    public async Task<Cliente?> BuscarPorId(int id)
    {
        var cliente = await _context.Clientes
        .AsNoTracking()
        .FirstOrDefaultAsync(c => c.Id == id);

        if (cliente == null)
            throw new NotFoundException($"Cliente com Id {id} não encontrado");

        return cliente;
    }

    public async Task<Cliente> Criar(Cliente cliente)
    {
        if (string.IsNullOrEmpty(cliente.Nome))
            throw new BusinessException("O nome do cliente é obrigatório para o cadastro.");

        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();
        return cliente;
    }

  
}