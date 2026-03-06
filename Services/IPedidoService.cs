using PizzariaApi.DTOs;
using PizzariaApi.Models;

namespace PizzariaApi.Services;

public interface IPedidoService
{
    Task<IEnumerable<Pedido>> ListarPedidos(string? status);
    Task<Pedido> CriarPedido(PedidoCreateDto dto);
    Task<bool> AlterarStatus(int id, string novoStatus);
    Task<RelatorioFaturamentoDto> GerarRelatorio();
}