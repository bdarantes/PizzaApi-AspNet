using PizzariaApi.Models.Enums;

namespace PizzariaApi.Models;

public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Categoria { get; set; } = "Pizza";
    public decimal PrecoUnitario { get; set; }
    public StatusProduto Status { get; set; } = StatusProduto.Ativo;
}