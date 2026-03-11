using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzariaApi.Data;
using PizzariaApi.Models;
using PizzariaApi.Services;
using PizzariaApi.Exceptions;

[ApiController]
[Route("api/[controller]")]
public class ProdutosController : ControllerBase
{
    private readonly IProdutoService _produtoService;

    public ProdutosController(IProdutoService produtoService)
    {
        _produtoService = produtoService;
    }

    /// <summary>
    /// Lista todos os produtos disponíveis e ativos.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Produto>>> Get()
    {
        return Ok(await _produtoService.ListarAtivos());
    }

    /// <summary>
    /// Busca um produto específico pelo seu ID.
    /// </summary>
    /// <param name="id">ID numérico do produto.</param>
    [HttpGet("{id}")]
    public async Task<ActionResult<Produto>> GetById(int id)
    {
        try
        {
        var produto = await _produtoService.BuscarPorId(id);
        return Ok(produto);
        }
        catch (NotFoundException ex)
        {
            
            return NotFound(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Cadastra um novo produto no cardápio.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Produto>> Post(Produto produto)
    {
        try
        {
        var novoProduto = await _produtoService.Criar(produto);
        return CreatedAtAction(nameof(GetById), new { id = novoProduto.Id }, novoProduto);
        }
        catch (BusinessException ex)
        {
            
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Atualiza os dados de um produto existente.
    /// </summary>
    /// <remarks>
    /// O ID do corpo do objeto deve ser igual ao ID da rota.
    /// </remarks>
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Produto produto)
    {
        try
        {
            await _produtoService.Atualizar(id, produto);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            
            return NotFound(new { message = ex.Message });
        }
        catch (BusinessException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, "Erro interno ao atualizar o produto.");
        }
    }

    /// <summary>
    /// Desativa um produto (Soft Delete).
    /// </summary>
    /// <remarks>
    /// O produto não é removido do banco, apenas marcado como inativo para preservar o histórico de pedidos.
    /// </remarks>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
        await _produtoService.SoftDelete(id);
        return Ok(new {message = "Produto desativado com sucesso."});
        }
        catch (NotFoundException ex)
        {
            
           return NotFound(new { message = ex.Message });
        }
        catch (BusinessException ex)
        {
            return BadRequest(new { message = ex.Message });
        }

    }
}