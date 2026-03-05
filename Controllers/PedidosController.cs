using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzariaApi.Data;
using PizzariaApi.Models;
using PizzariaApi.DTOs;
using PizzariaApi.Services;

namespace PizzariaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidosController : ControllerBase
{
    private readonly IPedidoService _pedidoService;

    public PedidosController(IPedidoService pedidoService)
    {
        _pedidoService = pedidoService;
    }

    [HttpGet]
    public async Task<IActionResult> Get(string? status)
    {
        return Ok(await _pedidoService.ListarPedidos(status));
    }

    [HttpGet("relatorio/faturamento")]
    public async Task<IActionResult> GerarRelatorio()
    {
        return Ok(await _pedidoService.GerarRelatorio());
    }

    [HttpPost]
    public async Task<IActionResult> Post(PedidoCreateDto dto)
    {
        try
        {
            var pedido = await _pedidoService.CriarPedido(dto);
            return CreatedAtAction(nameof(Get), new { id = pedido.Id }, pedido);
        }
        catch (Exception ex)
        {
            
            return BadRequest(ex.Message);
        }
    }
}