using AutoMapper;
using OrderService.Domain.Interfaces;
using OrderService.Domain.Models;
using OrderService.Application.DTOs;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Application.Services
{
    public class OrderAppService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderAppService(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderViewModel>> GetAllOrdersAsync(CancellationToken cancellationToken)
        {
            var orders = await _orderRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<OrderViewModel>>(orders);
        }

        public async Task<OrderViewModel?> GetOrderByIdAsync(int id, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(id, cancellationToken);
            return _mapper.Map<OrderViewModel>(order);
        }

        public async Task<OrderViewModel> CreateOrderAsync(OrderCreateDTO orderDto, CancellationToken cancellationToken)
        {
            var newOrder = _mapper.Map<Order>(orderDto);
            newOrder.Status = "Pending";

            await _orderRepository.AddAsync(newOrder, cancellationToken);

            return _mapper.Map<OrderViewModel>(newOrder);
        }

        public async Task UpdateOrderAsync(int id, OrderUpdateDTO orderDto, CancellationToken cancellationToken)
        {
            var existingOrder = await _orderRepository.GetByIdAsync(id, cancellationToken);
            if (existingOrder == null)
            {
                throw new KeyNotFoundException($"Order with ID {id} not found.");
            }

            _mapper.Map(orderDto, existingOrder);

            await _orderRepository.UpdateAsync(existingOrder, cancellationToken);
        }

        public async Task DeleteOrderAsync(int id, CancellationToken cancellationToken)
        {
            await _orderRepository.DeleteAsync(id, cancellationToken);
        }
    }
}