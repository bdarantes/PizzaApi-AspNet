namespace PizzariaApi.DTOs;

public class RelatorioFaturamentoDto
{
    public int TotalPedidos {get; set; }
    public decimal FaturamentoTotal { get; set; }
    public DateTime DataRelatorio { get; set; }
}