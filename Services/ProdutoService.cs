using Microsoft.EntityFrameworkCore;
using PizzariaApi.Data;
using PizzariaApi.Models;
using PizzariaApi.Models.Enums;
using PizzariaApi.Exceptions;

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
            .AsNoTracking()
            .Where(p => p.Status == StatusProduto.Ativo)
            .ToListAsync();    
    }

    public async Task<Produto?> BuscarPorId(int id)
    {
        var produto = await _context.Produtos
        .AsNoTracking()
        .FirstOrDefaultAsync(p => p.Id == id);

        if (produto == null)
            throw new NotFoundException($"O produto com Id {id} n/ao foi encontrado.");

        return produto;
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
            throw new ArgumentException("Os IDs fornecidos não coincidem.");
        
        _context.Entry(produto).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            
            if (!await ProdutoExiste(id))
                throw new NotFoundException($"Não foi possível atualizar. Produto {id} inexistente.");

            throw;
        }

    }

    public async Task<bool> SoftDelete(int id)
    {
        var produto = await _context.Produtos.FindAsync(id);

        if (produto == null)
            throw new NotFoundException($"Falha ao excluir. Produto {id} não encontrado.");

        produto.Status = StatusProduto.Inativo;
        await _context.SaveChangesAsync();
        return true;
    }

    private async Task<bool> ProdutoExiste(int id)
    {
        return await _context.Produtos.AnyAsync(e => e.Id == id);
    }
}