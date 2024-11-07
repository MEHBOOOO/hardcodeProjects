using MediatR;
using DeliveryService.Domain.Interfaces;
using DeliveryService.Application.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryService.Application.Handlers
{
    public class DeleteDeliveryRequestHandler : IRequestHandler<DeleteDeliveryRequestCommand, Unit>
    {
        private readonly IDeliveryRepository _deliveryRepository;

        public DeleteDeliveryRequestHandler(IDeliveryRepository deliveryRepository)
        {
            _deliveryRepository = deliveryRepository;
        }

        public async Task<Unit> Handle(DeleteDeliveryRequestCommand request, CancellationToken cancellationToken)
        {
            await _deliveryRepository.DeleteAsync(request.Id, cancellationToken);
            return Unit.Value;
        }
    }
}