using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NerdStore.Core.Communication.Mediator;
using NerdStore.Core.Messages.CommonMessages.IntegrationEvents;


namespace NerdStore.Catalogo.Domain.Events
{
    public class ProdutoEventHandler : 
        INotificationHandler<ProdutoAbaixoEstoqueEvent>,
        INotificationHandler<PedidoIniciadoEvent>,
        INotificationHandler<PedidoProcessamentoCanceladoEvent>
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IEstoqueService _estoqueService;
        private readonly IMediatorHandler _mediator;

        public ProdutoEventHandler(IProdutoRepository produtoRepository, IEstoqueService estoqueService, IMediatorHandler mediator)
        {
            _produtoRepository = produtoRepository;
            _estoqueService = estoqueService;
            _mediator = mediator;
        }

        public async Task Handle(ProdutoAbaixoEstoqueEvent mensagem, CancellationToken cancellationToken)
        {
            var produto = await _produtoRepository.ObterPorId(mensagem.AggregateId);

            //Enviar um email para aquisição para de mais produtos.
        }

        public async Task Handle(PedidoIniciadoEvent mensagem, CancellationToken cancellationToken)
        {
            var result = await _estoqueService.DebitarListaProdutosPedido(mensagem.ProdutosPedido);

            if (result)
            {
                await _mediator.PublicarEvento(new PedidoEstoqueConfirmadoEvent(mensagem.PedidoId, mensagem.ClienteId, mensagem.Total,
                                                                                mensagem.ProdutosPedido,mensagem.NomeCartao,
                                                                                mensagem.NumeroCartao,mensagem.ExpiracaoCartao,
                                                                                mensagem.CvvCartao));
            }
            else
            {
                await _mediator.PublicarEvento(new PedidoEstoqueRejeitadoEvent(mensagem.PedidoId, mensagem.ClienteId));
            }
        }

        public async Task Handle(PedidoProcessamentoCanceladoEvent mensagem, CancellationToken cancellationToken)
        {
            await _estoqueService.ReporListaProdutosPedido(mensagem.ProdutosPedido);
        }
    }
}