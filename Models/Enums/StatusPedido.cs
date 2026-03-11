namespace PizzariaApi.Models.Enums;

public enum StatusPedido : int
{
    Pendente = 1,
    EmPreparo = 2,
    Pronto = 3,
    EmEntrega = 4,
    Entregue = 5,
    Cancelado = 0
}