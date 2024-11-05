using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DeliveryService.Data;
using DeliveryService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using DeliveryService.DataTransferObjects;

namespace DeliveryService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeliveryRequestsController : ControllerBase
    {
        private readonly DeliveryContext _context;

        public DeliveryRequestsController(DeliveryContext context)
        {
            _context = context;
        }

// get запрос
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeliveryRequestViewModel>>> GetDeliveryRequests(CancellationToken cancellationToken)
        {
            // return await _context.DeliveryRequests.ToListAsync();
            var deliveryRequests = await _context.DeliveryRequests.Select(dr => new DeliveryRequestViewModel
                {
                    Id = dr.Id,
                    OrderId = dr.OrderId,
                    Status = dr.Status
                }).ToListAsync(cancellationToken);

            return Ok(deliveryRequests);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DeliveryRequestViewModel>> GetDeliveryRequest(int id, CancellationToken cancellationToken)
        {
            var deliveryRequest = await _context.DeliveryRequests.FindAsync(new object[] {id}, cancellationToken);
            // 
            if (deliveryRequest == null)
            {
                return NotFound();
            }
            var deliveryRequestViewModel = new DeliveryRequestViewModel
            {
                Id = deliveryRequest.Id,
                OrderId = deliveryRequest.OrderId,
                Status = deliveryRequest.Status
            };
                return Ok(deliveryRequestViewModel);
            // return deliveryRequest;
        }

// post запрос
        [HttpPost]
        public async Task<ActionResult<DeliveryRequestViewModel>> CreateDeliveryRequest(
            [FromBody] CreateDeliveryRequestDTO createDeliveryRequestDTO, CancellationToken cancellationToken)
        // {
        //     _context.DeliveryRequests.Add(deliveryRequest);
        //     await _context.SaveChangesAsync();

        //     return CreatedAtAction(nameof(GetDeliveryRequest), new { id = deliveryRequest.Id }, deliveryRequest);
        // }
        {
            var deliveryRequest = new DeliveryRequest
            {
                OrderId = createDeliveryRequestDTO.OrderId,
                Status = createDeliveryRequestDTO.Status
            };
            _context.DeliveryRequests.Add(deliveryRequest);
            await _context.SaveChangesAsync(cancellationToken);

            var createdDeliveryRequestViewModel = new DeliveryRequestViewModel
            {
                Id = deliveryRequest.Id,
                OrderId = deliveryRequest.OrderId,
                Status = deliveryRequest.Status
            };
                return CreatedAtAction(nameof(GetDeliveryRequest), new { id = deliveryRequest.Id }, createdDeliveryRequestViewModel);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDeliveryRequest(int id, [FromBody] UpdateDeliveryRequestDTO updateDeliveryRequestDTO, CancellationToken cancellationToken)
        {
            var existingDeliveryRequest = await _context.DeliveryRequests.FindAsync(new object[] { id }, cancellationToken);
            if (existingDeliveryRequest == null)
            {
                return NotFound();
            }

            existingDeliveryRequest.OrderId = updateDeliveryRequestDTO.OrderId;
            existingDeliveryRequest.Status = updateDeliveryRequestDTO.Status;

            await _context.SaveChangesAsync(cancellationToken);

            return NoContent();            
        }

// delete запрос
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeliveryRequest(int id, CancellationToken cancellationToken)
        {
            var deliveryRequest = await _context.DeliveryRequests.FindAsync(new object[] {id}, cancellationToken);
            if (deliveryRequest == null)
            {
                return NotFound();
            }

            _context.DeliveryRequests.Remove(deliveryRequest);
            await _context.SaveChangesAsync(cancellationToken);

            return NoContent();
        }

        private bool DeliveryRequestExists(int id)
        {
            return _context.DeliveryRequests.Any(e => e.Id == id);
        }
    }
}