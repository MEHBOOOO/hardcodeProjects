using MediatR;
using DeliveryService.Application.DTOs;

namespace DeliveryService.Application.Queries
{
    public class GetDeliveryRequestByIdQuery : IRequest<DeliveryRequestViewModel>
    {
        public int Id { get; set; }
    }
}