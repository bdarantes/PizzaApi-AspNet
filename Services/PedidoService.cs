using Microsoft.EntityFrameworkCore;
using PizzariaApi.Data;
using PizzariaApi.DTOs;
using PizzariaApi.Models;
using PizzariaApi.Exceptions;
using PizzariaApi.Models.Enums;

namespace PizzariaApi.Services;

public class PedidoService : IPedidoService
{
    private readonly AppDbContext _context;

    public PedidoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Pedido>> ListarPedidos(StatusPedido? status)
    {
        var query = _context.Pedidos
            .AsNoTracking()
            .Include(p => p.Cliente)
            .Include(p =>p.Produto)
            .AsQueryable();

        if (status.HasValue)
            query = query.Where(p => p.Status == status.Value);

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
            Status = StatusPedido.Pendente,
            Total = produto.PrecoUnitario
        };

        _context.Pedidos.Add(novoPedido);
        await _context.SaveChangesAsync();
        return novoPedido;
    }

    public async Task<bool> AlterarStatus(int id,  StatusPedido novoStatus)
    {
        var pedido = await _context.Pedidos.FindAsync(id);
        
        if (pedido == null)
            throw new NotFoundException($"Pedido nº {id} não encontrado para alteração de status.");


        if (pedido.Status == StatusPedido.Cancelado)
            throw new BusinessException("Não é possível alterar o status de um pedido cancelado.");

        if (pedido.Status == StatusPedido.Entregue && novoStatus != StatusPedido.Entregue)
            throw  new BusinessException("Pedidos entregues não podem ter o status alterado.");
        
        pedido.Status = novoStatus;
        await _context.SaveChangesAsync();

        return true;
    

    }

    public async Task<RelatorioFaturamentoDto> GerarRelatorio()
    {
        var pedidos = await _context.Pedidos
        .AsNoTracking()
        .Where(p => p.Status !=  StatusPedido.Cancelado)
        .ToListAsync();
        return new RelatorioFaturamentoDto
        {
            TotalPedidos = pedidos.Count,
            FaturamentoTotal = pedidos.Sum(p => p.Total),
            DataRelatorio = DateTime.Now
        };
    }
}