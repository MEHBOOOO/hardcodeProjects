using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DeliveryService.Data;
using DeliveryService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<ActionResult<IEnumerable<DeliveryRequest>>> GetDeliveryRequests()
        {
            return await _context.DeliveryRequests.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DeliveryRequest>> GetDeliveryRequest(int id)
        {
            var deliveryRequest = await _context.DeliveryRequests.FindAsync(id);

            if (deliveryRequest == null)
            {
                return NotFound();
            }

            return deliveryRequest;
        }

// post запрос
        [HttpPost]
        public async Task<ActionResult<DeliveryRequest>> PostDeliveryRequest(DeliveryRequest deliveryRequest)
        {
            _context.DeliveryRequests.Add(deliveryRequest);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDeliveryRequest), new { id = deliveryRequest.Id }, deliveryRequest);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDeliveryRequest(int id, DeliveryRequest deliveryRequest)
        {
            if (id != deliveryRequest.Id)
            {
                return BadRequest();
            }

            _context.Entry(deliveryRequest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeliveryRequestExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

// delete запрос
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeliveryRequest(int id)
        {
            var deliveryRequest = await _context.DeliveryRequests.FindAsync(id);
            if (deliveryRequest == null)
            {
                return NotFound();
            }

            _context.DeliveryRequests.Remove(deliveryRequest);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DeliveryRequestExists(int id)
        {
            return _context.DeliveryRequests.Any(e => e.Id == id);
        }
    }
}