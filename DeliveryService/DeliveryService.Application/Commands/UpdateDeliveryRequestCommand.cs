using MediatR;
using DeliveryService.Application.DTOs;

namespace DeliveryService.Application.Commands
{
    public class UpdateDeliveryRequestCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public required string Address { get; set; }
        public required string Status { get; set; }
    }
}