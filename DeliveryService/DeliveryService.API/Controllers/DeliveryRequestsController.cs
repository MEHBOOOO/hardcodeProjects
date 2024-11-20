using MediatR;
using Microsoft.AspNetCore.Mvc;
using DeliveryService.Application.Commands;
using DeliveryService.Application.Queries;
using DeliveryService.Application.DTOs;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeliveryRequestsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DeliveryRequestsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DeliveryRequestViewModel>> GetDeliveryRequest(int id, CancellationToken cancellationToken)
        {
            var query = new GetDeliveryRequestByIdQuery { Id = id };
            var deliveryRequest = await _mediator.Send(query, cancellationToken);
            if (deliveryRequest == null)
            {
                return NotFound();
            }

            return Ok(deliveryRequest);
        }

        [HttpPost]
        public async Task<ActionResult<DeliveryRequestViewModel>> CreateDeliveryRequest([FromBody] CreateDeliveryRequestCommand command, CancellationToken cancellationToken)
        {
            var createdDeliveryRequest = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetDeliveryRequest), new { id = createdDeliveryRequest.Id }, createdDeliveryRequest);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateDeliveryRequest(int id, [FromBody] UpdateDeliveryRequestCommand command, CancellationToken cancellationToken)
        {
            try
            {
                command.Id = id;
                await _mediator.Send(command, cancellationToken);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDeliveryRequest(int id, CancellationToken cancellationToken)
        {
            var command = new DeleteDeliveryRequestCommand { Id = id };
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
    }
}