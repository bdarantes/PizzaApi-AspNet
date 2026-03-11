using Microsoft.EntityFrameworkCore;
using PizzariaApi.Data;
using PizzariaApi.DTOs;
using PizzariaApi.Models;
using PizzariaApi.Exceptions;

namespace PizzariaApi.Services;

public class PedidoService : IPedidoService
{
    private readonly AppDbContext _context;

    public PedidoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Pedido>> ListarPedidos(string? status)
    {
        var query = _context.Pedidos
            .AsNoTracking()
            .Include(p => p.Cliente)
            .Include(p =>p.Produto)
            .AsQueryable();
        
        if (!string.IsNullOrEmpty(status)) 
            query = query.Where(p => p.Status == status);

        return await query.ToListAsync();

    }

    public async Task<Pedido> CriarPedido(PedidoCreateDto dto)
    {
        var produto = await _context.Produtos.FindAsync(dto.ProdutoId);
        
        if (produto == null)
            throw new NotFoundException("Não foi possível criar o pedido: Produto inexistente.");

        if (produto.Status == Models.Enums.StatusProduto.Inativo)
            throw new BusinessException("Este produto não está disponível para venda no momento.");

        var novoPedido = new Pedido
        {
            ClienteId = dto.ClienteId,
            ProdutoId = dto.ProdutoId,
            DataPedido = DateTime.Now,
            Status = "Pendente",
            Total = produto.PrecoUnitario
        };

        _context.Pedidos.Add(novoPedido);
        await _context.SaveChangesAsync();
        return novoPedido;
    }

    public async Task<bool> AlterarStatus(int id, string novoStatus)
    {
        var pedido = await _context.Pedidos.FindAsync(id);
        
        if (pedido == null)
            throw new NotFoundException($"Pedido nº {id} não encontrado para alteração de status.");

        var statusPermitidos = new[] { "Pendente", "Em preparo", "Pronto", "Entregue", "Cancelado" };
        
        if (!statusPermitidos.Contains(novoStatus))
            throw new BusinessException($"O status '{novoStatus}' não é um status válido para o sistema.");

        if (pedido.Status == "Entregue" && novoStatus == "Cancelado")
            throw new BusinessException("Não é possível cancelar um pedido que já foi entregue ao cliente");

        pedido.Status = novoStatus;
        await _context.SaveChangesAsync();
        return true;

    }

    public async Task<RelatorioFaturamentoDto> GerarRelatorio()
    {
        var pedidos = await _context.Pedidos
        .AsNoTracking()
        .Where(p => p.Status != "Cancelado")
        .ToListAsync();
        return new RelatorioFaturamentoDto
        {
            TotalPedidos = pedidos.Count,
            FaturamentoTotal = pedidos.Sum(p => p.Total),
            DataRelatorio = DateTime.Now
        };
    }
}