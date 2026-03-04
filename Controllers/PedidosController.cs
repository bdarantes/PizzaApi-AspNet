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
      var novoPedido = new Pedido
      {
          ClienteId = pedidoDto.ClienteId,
          ProdutoId = pedidoDto.ProdutoId,
          DataPedido = DateTime.Now

      };

      _context.Pedidos.Add(novoPedido);
      await _context.SaveChangesAsync();

      return CreatedAtAction(nameof(GetPedidos), new { id = novoPedido.Id }, novoPedido);
    }
}