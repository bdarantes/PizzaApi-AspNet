using Microsoft.EntityFrameworkCore;
using PizzariaApi.Data;
using PizzariaApi.Models;
using PizzariaApi.Models.Enums;

namespace PizzariaApi.Services;

public class ProdutoService : IProdutoService
{
    private readonly AppDbContext _context;

    public ProdutoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Produto>> ListarAtivos()
    {
        return await _context.Produtos
            .Where(p => p.Status == StatusProduto.Ativo)
            .ToListAsync();    
    }

    public async Task<Produto?> BuscarPorId(int id)
    {
        return await _context.Produtos.FindAsync(id);
    }

    public async Task<Produto> Criar(Produto produto)
    {
        produto.Status = StatusProduto.Ativo;

        _context.Produtos.Add(produto);
        await _context.SaveChangesAsync();
        return produto;
    }

    public async Task Atualizar(int id, Produto produto)
    {
        if (id != produto.Id)
            throw new Exception("IDs não coincidem.");
        
        _context.Entry(produto).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            
            if (!await ProdutoExiste(id))
                throw new Exception("Produto não encontrado");

            throw;
        }

    }

    public async Task<bool> SoftDelete(int id)
    {
        var produto = await _context.Produtos.FindAsync(id);

        if (produto == null)
            return false;

        produto.Status = StatusProduto.Inativo;
        await _context.SaveChangesAsync();
        return true;
    }

    private async Task<bool> ProdutoExiste(int id)
    {
        return await _context.Produtos.AnyAsync(e => e.Id == id);
    }
}