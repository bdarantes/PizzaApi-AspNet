using Microsoft.AspNetCore.Mvc;
using PizzariaApi.Models;
using PizzariaApi.Services;
using PizzariaApi.Exceptions;

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

    /// <summary>
    /// Retorna a lista de todos os clientes cadastrados.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cliente>>> Get()
    {
        var clientes = await _clienteService.ListarTodos();
        return Ok(clientes);
    }

    /// <summary>
    /// Cadastra um novo cliente no sistema.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Cliente>> Post(Cliente cliente)
    {
        try
        {
            var novoCliente = await _clienteService.Criar(cliente);
            
            return CreatedAtAction(nameof(Get), new { id = novoCliente.Id}, novoCliente);
        }
        catch (BusinessException ex)
        {
            
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, "Erro ao processar o cadastro do cliente");
        }
    }
}