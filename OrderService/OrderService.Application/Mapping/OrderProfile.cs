using AutoMapper;
using OrderService.Domain.Models;
using OrderService.Application.DTOs;

namespace OrderService.Application.Mapping
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderViewModel>();
            CreateMap<OrderCreateDTO, Order>();
            CreateMap<OrderUpdateDTO, Order>();
        }
    }
}