using Microsoft.AspNetCore.Mvc;
using PizzariaApi.Models.Enums;

namespace PizzariaApi.Models;

public class Pedido
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public Cliente? Cliente { get; set; }
    public int ProdutoId { get; set; }
    public Produto? Produto { get; set; }
    public DateTime DataPedido { get; set; } = DateTime.Now;
    public decimal Total { get; set; }
    public StatusPedido Status { get; set; } = StatusPedido.Pendente;

  
}