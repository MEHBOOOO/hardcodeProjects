using MediatR;
using DeliveryService.Application.DTOs;

namespace DeliveryService.Application.Commands
{
    public class CreateDeliveryRequestCommand : IRequest<DeliveryRequestViewModel>
    {
        public required string Address { get; set; }
    }
}