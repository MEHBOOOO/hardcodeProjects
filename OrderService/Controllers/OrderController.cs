using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OrderService.DataTransferObjects;
using SharedEvents;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderContext _context;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrdersController(OrderContext context, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
        }

        // get запрос на все заказы
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderViewModel>>> GetOrders(CancellationToken cancellationToken)
        {
            var orders = await _context.Orders
                .Select(order => new OrderViewModel
                {
                    Id = order.Id,
                    ProductName = order.ProductName,
                    Quantity = order.Quantity,
                    Price = order.Price
                })
                .ToListAsync(cancellationToken);

            return Ok(orders);
        }

        // get запрос по ID
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderViewModel>> GetOrder(int id, CancellationToken cancellationToken)
        {
            var order = await _context.Orders.FindAsync(new object[] { id }, cancellationToken);
            if (order == null)
            {
                return NotFound();
            }

            var orderViewModel = new OrderViewModel
            {
                Id = order.Id,
                ProductName = order.ProductName,
                Quantity = order.Quantity,
                Price = order.Price
            };

            return Ok(orderViewModel);
        }

        // post запрос на создание нового заказа
        [HttpPost]
        public async Task<ActionResult<OrderViewModel>> CreateOrder([FromBody] OrderCreateDTO newOrderDTO, CancellationToken cancellationToken)
        {
            try
            {
                if (newOrderDTO == null || string.IsNullOrWhiteSpace(newOrderDTO.ProductName))
                {
                    return BadRequest("ProductName is required.");
                }

                var newOrder = new Order
                {
                    ProductName = newOrderDTO.ProductName,
                    Quantity = newOrderDTO.Quantity,
                    Price = newOrderDTO.Price
                };

                _context.Orders.Add(newOrder);
                await _context.SaveChangesAsync(cancellationToken);

// test
                await _publishEndpoint.Publish(new SharedEvents.OrderCreatedEvent
                {
                    OrderId = newOrder.Id,
                    ProductName = newOrder.ProductName,
                    Quantity = newOrder.Quantity,
                    Price = newOrder.Price
                }, cancellationToken);
                // throw new Exception("error test");

                var createdOrderViewModel = new OrderViewModel
                {
                    Id = newOrder.Id,
                    ProductName = newOrder.ProductName,
                    Quantity = newOrder.Quantity,
                    Price = newOrder.Price
                };

                return CreatedAtAction(nameof(GetOrder), new { id = newOrder.Id }, createdOrderViewModel);
            }
            catch (Exception ex)
            {
// ошибка
                Console.WriteLine($"Error creating order: {ex.Message}");
                return StatusCode(500, "An error occurred while creating the order.");
            }
        }

        // put запрос на обновление заказа
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOrder(int id, [FromBody] OrderUpdateDTO updatedOrderDTO, CancellationToken cancellationToken)
        {
            var existingOrder = await _context.Orders.FindAsync(new object[] { id }, cancellationToken);
            if (existingOrder == null)
            {
                return NotFound();
            }

            existingOrder.ProductName = updatedOrderDTO.ProductName;
            existingOrder.Quantity = updatedOrderDTO.Quantity;
            existingOrder.Price = updatedOrderDTO.Price;

            await _context.SaveChangesAsync(cancellationToken);

            return NoContent();
        }

        // delete запрос на удаление заказа
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(int id, CancellationToken cancellationToken)
        {
            var order = await _context.Orders.FindAsync(new object[] { id }, cancellationToken);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync(cancellationToken);
            return NoContent();
        }
    }
}