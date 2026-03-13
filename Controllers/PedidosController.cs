using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzariaApi.Data;
using PizzariaApi.Models;
using PizzariaApi.DTOs;
using PizzariaApi.Services;
using PizzariaApi.Models.Enums;
using PizzariaApi.Exceptions;

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

    /// <summary>
    /// Lista todos os produtos disponíveis e ativos.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] StatusPedido? status)
    {
        return Ok(await _pedidoService.ListarPedidos(status));
    }

    /// <summary>
    /// Registra um novo pedido na pizzaria.
    /// </summary>
    /// <remarks>
    /// O sistema valida se o produto e o cliente existem e se o produto está ativo antes de finalizar.
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Post(PedidoCreateDto dto)
    {
        var pedido = await _pedidoService.CriarPedido(dto);
        return CreatedAtAction(nameof(Get), new { id = pedido.Id }, pedido);

    }

    /// <summary>
    /// Atualiza apenas o status de um pedido.
    /// </summary>
    /// <param name="id">ID do pedido.</param>
    /// <param name="novoStatus">Ex: 'Em Preparo', 'Pronto', 'Entregue', 'Cancelado'.</param>
    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] StatusPedido novoStatus)
    {
        await _pedidoService.AlterarStatus(id, novoStatus);
            
        return Ok(new { message = $"Status do pedido {id} atualizado com sucesso. "});
    
    }
    /// <summary>
    /// Gera um relatório financeiro com o faturamento total da pizzaria.
    /// </summary>
    /// <remarks>
    /// Este cálculo ignora pedidos com status 'Cancelado'.
    /// </remarks>
    /// <returns>Retorna o total de pedidos e a soma dos valores.</returns>
    [HttpGet("relatorio/faturamento")]
    public async Task<IActionResult> GerarRelatorio()
    {
        return Ok(await _pedidoService.GerarRelatorio());
    }


}