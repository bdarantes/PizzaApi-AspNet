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
    public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidos()
    {
        return await _context.Pedidos
            .Include(p => p.Cliente)
            .ToListAsync();
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
}