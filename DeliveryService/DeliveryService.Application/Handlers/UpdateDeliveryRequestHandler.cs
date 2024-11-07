using MediatR;
using DeliveryService.Domain.Interfaces;
using DeliveryService.Application.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryService.Application.Handlers
{
    public class UpdateDeliveryRequestHandler : IRequestHandler<UpdateDeliveryRequestCommand, Unit>
    {
        private readonly IDeliveryRepository _deliveryRepository;

        public UpdateDeliveryRequestHandler(IDeliveryRepository deliveryRepository)
        {
            _deliveryRepository = deliveryRepository;
        }

        public async Task<Unit> Handle(UpdateDeliveryRequestCommand request, CancellationToken cancellationToken)
        {
            var deliveryRequest = await _deliveryRepository.GetByIdAsync(request.Id, cancellationToken);
            if (deliveryRequest == null)
            {
                throw new KeyNotFoundException($"Delivery request with ID {request.Id} not found.");
            }

            deliveryRequest.Address = request.Address;
            deliveryRequest.Status = request.Status;

            await _deliveryRepository.UpdateAsync(deliveryRequest, cancellationToken);

            return Unit.Value;
        }
    }
}