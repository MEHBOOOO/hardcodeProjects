using MediatR;
using DeliveryService.Domain.Interfaces;
using DeliveryService.Domain.Models;
using DeliveryService.Application.Commands;
using DeliveryService.Application.DTOs;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryService.Application.Handlers
{
    public class CreateDeliveryRequestHandler : IRequestHandler<CreateDeliveryRequestCommand, DeliveryRequestViewModel>
    {
        private readonly IDeliveryRepository _deliveryRepository;

        public CreateDeliveryRequestHandler(IDeliveryRepository deliveryRepository)
        {
            _deliveryRepository = deliveryRepository;
        }

        public async Task<DeliveryRequestViewModel> Handle(CreateDeliveryRequestCommand request, CancellationToken cancellationToken)
        {
            var deliveryRequest = new DeliveryRequest
            {
                Address = request.Address,
                Status = "Pending"
            };

            await _deliveryRepository.AddAsync(deliveryRequest, cancellationToken);

            return new DeliveryRequestViewModel
            {
                Id = deliveryRequest.Id,
                Address = deliveryRequest.Address,
                Status = deliveryRequest.Status
            };
        }
    }
}