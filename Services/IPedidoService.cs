using PizzariaApi.DTOs;
using PizzariaApi.Models;
using PizzariaApi.Models.Enums;

namespace PizzariaApi.Services;

public interface IPedidoService
{
    Task<IEnumerable<Pedido>> ListarPedidos(StatusPedido? status);
    Task<Pedido> CriarPedido(PedidoCreateDto dto);
    Task<bool> AlterarStatus(int id, StatusPedido novoStatus);
    Task<RelatorioFaturamentoDto> GerarRelatorio();
}