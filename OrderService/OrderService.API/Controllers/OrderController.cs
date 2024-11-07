using MassTransit;
using Microsoft.AspNetCore.Mvc;
using OrderService.Application.DTOs;
using OrderService.Application.Services;
using OrderService.Domain.Events;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderAppService _orderAppService;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(OrderAppService orderAppService, IPublishEndpoint publishEndpoint, ILogger<OrdersController> logger)
        {
            _orderAppService = orderAppService;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderViewModel>>> GetOrders(CancellationToken cancellationToken)
        {
            var orders = await _orderAppService.GetAllOrdersAsync(cancellationToken);
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderViewModel>> GetOrder(int id, CancellationToken cancellationToken)
        {
            var order = await _orderAppService.GetOrderByIdAsync(id, cancellationToken);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult<OrderViewModel>> CreateOrder([FromBody] OrderCreateDTO newOrderDTO, CancellationToken cancellationToken)
        {
            try
            {
                if (newOrderDTO == null || string.IsNullOrWhiteSpace(newOrderDTO.ProductName))
                {
                    return BadRequest("ProductName is required.");
                }

                var createdOrder = await _orderAppService.CreateOrderAsync(newOrderDTO, cancellationToken);

                await _publishEndpoint.Publish(new OrderCreatedEvent
                {
                    OrderId = createdOrder.Id,
                    ProductName = createdOrder.ProductName,
                    Quantity = createdOrder.Quantity,
                    Price = createdOrder.Price,
                    NewStatus = "Pending"
                }, cancellationToken);

                return CreatedAtAction(nameof(GetOrder), new { id = createdOrder.Id }, createdOrder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order");
                return StatusCode(500, "An error occurred while creating the order.");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOrder(int id, [FromBody] OrderUpdateDTO updatedOrderDTO, CancellationToken cancellationToken)
        {
            try
            {
                await _orderAppService.UpdateOrderAsync(id, updatedOrderDTO, cancellationToken);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating order");
                return StatusCode(500, "An error occurred while updating the order.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(int id, CancellationToken cancellationToken)
        {
            try
            {
                await _orderAppService.DeleteOrderAsync(id, cancellationToken);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting order");
                return StatusCode(500, "An error occurred while deleting the order.");
            }
        }
    }
}