using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzariaApi.Data;
using PizzariaApi.Models;

[ApiController]
[Route("api/[controller]")]
public class ProdutosController : ControllerBase
{
    private readonly AppDbContext _context;
    public ProdutosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Produto>>> GetProdutos()
    {
        return await _context.Produtos.ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Produto>> PostProduto(Produto produto)
    {
        _context.Produtos.Add(produto);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProdutos), new { id = produto.Id }, produto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutProduto(int id, Produto produto)
    {
        if (id != produto.Id)
            return BadRequest("O ID do produto não coincide.");

        _context.Entry(produto).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            
            if (!_context.Produtos.Any(e => e.Id == id))
            {
                return NotFound(new { message = $"Erro de Concorrência: O produto com ID {id} não existe mais"});
            }
            else
            {
                return StatusCode(500, "Ocorreu um erro ao atualizar o produto. O registro pode ter sido alterado por outro usuário");
            }
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduto(int id)
    {
        var produto = await _context.Produtos.FindAsync(id);
        if (produto == null)
            return NotFound("Produto não encontrado para exclusão.");

        _context.Produtos.Remove(produto);
        await _context.SaveChangesAsync();

        return Ok(new { message = $"PRoduto '{produto.Nome}' removido com sucesso. "});
    }
}