using Microsoft.AspNetCore.Mvc;
using PizzariaApi.Models;
using PizzariaApi.Services;

namespace PizzariaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController :ControllerBase
{
    private readonly IClienteService _clienteService;
    public ClientesController(IClienteService clienteService)
    {
        _clienteService = clienteService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cliente>>> Get()
    {
        return Ok(await _clienteService.ListarTodos());
    }

    [HttpPost]
    public async Task<ActionResult<Cliente>> Post(Cliente cliente)
    {
        var novoCliente = await _clienteService.Criar(cliente);
        return CreatedAtAction(nameof(Get), new { id = novoCliente.Id}, novoCliente);
    }
}