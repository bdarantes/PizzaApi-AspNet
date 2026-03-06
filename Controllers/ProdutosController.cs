using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzariaApi.Data;
using PizzariaApi.Models;
using PizzariaApi.Services;

[ApiController]
[Route("api/[controller]")]
public class ProdutosController : ControllerBase
{
    private readonly IProdutoService _produtoService;

    public ProdutosController(IProdutoService produtoService)
    {
        _produtoService = produtoService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Produto>>> Get()
    {
        return Ok(await _produtoService.ListarAtivos());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Produto>> GetById(int id)
    {
        var produto = await _produtoService.BuscarPorId(id);
        if (produto == null)
            return NotFound("Produto não encontrado.");
        
        return Ok(produto);
    }

    [HttpPost]
    public async Task<ActionResult<Produto>> Post(Produto produto)
    {
        var novoProduto = await _produtoService.Criar(produto);
        return CreatedAtAction(nameof(GetById), new { id = novoProduto.Id }, novoProduto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Produto produto)
    {
        try
        {
            await _produtoService.Atualizar(id, produto);
            return NoContent();
        }
        catch (Exception ex)
        {
            
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var sucesso = await _produtoService.SoftDelete(id);

        if (!sucesso)
            return NotFound("Produto não encontrado.");

        return Ok(new {message = "Produtodesativado com sucesso."});

    }
}