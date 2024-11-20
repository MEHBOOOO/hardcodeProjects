using MediatR;

namespace DeliveryService.Application.Commands
{
    public class DeleteDeliveryRequestCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}