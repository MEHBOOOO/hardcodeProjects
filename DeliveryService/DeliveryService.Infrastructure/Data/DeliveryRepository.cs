using Microsoft.EntityFrameworkCore;
using DeliveryService.Domain.Interfaces;
using DeliveryService.Domain.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryService.Infrastructure.Data
{
    public class DeliveryRepository : IDeliveryRepository
    {
        private readonly DeliveryContext _context;

        public DeliveryRepository(DeliveryContext context)
        {
            _context = context;
        }

        public async Task<DeliveryRequest?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.DeliveryRequests.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<IEnumerable<DeliveryRequest>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.DeliveryRequests.ToListAsync(cancellationToken);
        }

        public async Task AddAsync(DeliveryRequest deliveryRequest, CancellationToken cancellationToken)
        {
            _context.DeliveryRequests.Add(deliveryRequest);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(DeliveryRequest deliveryRequest, CancellationToken cancellationToken)
        {
            _context.DeliveryRequests.Update(deliveryRequest);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var deliveryRequest = await _context.DeliveryRequests.FindAsync(new object[] { id }, cancellationToken);
            if (deliveryRequest != null)
            {
                _context.DeliveryRequests.Remove(deliveryRequest);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}