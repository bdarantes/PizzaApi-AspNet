using Microsoft.AspNetCore.Mvc;

namespace PizzariaApi.Models;

public class Pedido
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public DateTime DataPedido { get; set; } = DateTime.Now;
    public string Status { get; set; } = "Pendente";
    public decimal Total { get; set; }

    public Cliente? Cliente { get; set; }
}