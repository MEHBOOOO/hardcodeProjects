using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DeliveryService.Domain.Models;

namespace DeliveryService.Domain.Interfaces
{
    public interface IDeliveryRepository
    {
        Task<DeliveryRequest?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<DeliveryRequest>> GetAllAsync(CancellationToken cancellationToken);
        Task AddAsync(DeliveryRequest deliveryRequest, CancellationToken cancellationToken);
        Task UpdateAsync(DeliveryRequest deliveryRequest, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
    }
}