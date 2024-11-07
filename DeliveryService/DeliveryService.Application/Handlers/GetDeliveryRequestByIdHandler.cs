using MediatR;
using DeliveryService.Domain.Interfaces;
using DeliveryService.Application.Queries;
using DeliveryService.Application.DTOs;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryService.Application.Handlers
{
    public class GetDeliveryRequestByIdHandler : IRequestHandler<GetDeliveryRequestByIdQuery, DeliveryRequestViewModel?>
    {
        private readonly IDeliveryRepository _deliveryRepository;

        public GetDeliveryRequestByIdHandler(IDeliveryRepository deliveryRepository)
        {
            _deliveryRepository = deliveryRepository;
        }

        public async Task<DeliveryRequestViewModel?> Handle(GetDeliveryRequestByIdQuery request, CancellationToken cancellationToken)
        {
            var deliveryRequest = await _deliveryRepository.GetByIdAsync(request.Id, cancellationToken);
            if (deliveryRequest == null)
            {
                return null;
            }

            return new DeliveryRequestViewModel
            {
                Id = deliveryRequest.Id,
                Address = deliveryRequest.Address,
                Status = deliveryRequest.Status
            };
        }
    }
}