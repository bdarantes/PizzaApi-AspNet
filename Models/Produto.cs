namespace PizzariaApi.Models;

public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Categoria { get; set; } = "Pizza";
    public decimal PrecoUnitario { get; set; }
}