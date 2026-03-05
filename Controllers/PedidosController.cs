using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzariaApi.Data;
using PizzariaApi.Models;
using PizzariaApi.DTOs;

namespace PizzariaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidosController : ControllerBase
{
    private readonly AppDbContext _context;

    public PedidosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidos([FromQuery] string? status)
    {
        var query = _context.Pedidos
            .Include(p => p.Cliente)
            .Include(p => p.Produto)
            .AsQueryable();

        if (!string.IsNullOrEmpty(status))
            query = query.Where(p => p.Status == status);

        return await query.ToListAsync();

    }

    [HttpPost]
    public async Task<ActionResult<Pedido>> PostPedido(PedidoCreateDto pedidoDto)
    {
        var produto = await _context.Produtos.FindAsync(pedidoDto.ProdutoId);

        if (produto == null)
            return NotFound("Produto não encontrado, Verifique o ID.");

        
        var novoPedido = new Pedido
        {
            ClienteId = pedidoDto.ClienteId,
            ProdutoId = pedidoDto.ProdutoId,
            DataPedido = DateTime.Now,
            Status = "Pendente",
            Total = produto.PrecoUnitario

        };

      _context.Pedidos.Add(novoPedido);
      await _context.SaveChangesAsync();

      return CreatedAtAction(nameof(GetPedidos), new { id = novoPedido.Id }, novoPedido);
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, PedidoStatusUpdateDto statusDto)
    {
        var statusPermitidos = new List<string> { "Pendente", "Preparando", "Pronto", "Entregue", "Cancelado" };

        if (!statusPermitidos.Contains(statusDto.Status))
            return BadRequest($"Status inválido. Use apenas: {string.Join(", ",statusPermitidos)}");

        var pedido = await _context.Pedidos.FindAsync(id);

        if (pedido == null)
            return NotFound("Pedido não encotrado.");

        pedido.Status = statusDto.Status;

        await _context.SaveChangesAsync();

        return Ok(new { message = $"Status do pedido {id} atualizado para: {pedido.Status}"});
    }

    [HttpGet("relatorio/faturamento")]
    public async Task<ActionResult<RelatorioFaturamentoDto>> GetFaturamento()
    {
        var pedidosValidos = await _context.Pedidos
            .Where(p => p.Status != "Cancelado")
            .ToListAsync();

        var relatorio = new RelatorioFaturamentoDto
        {
            TotalPedidos = pedidosValidos.Count,
            FaturamentoTotal = pedidosValidos.Sum(p => p.Total),
            DataRelatorio = DateTime.Now
        };

        return Ok(relatorio);
    }
}