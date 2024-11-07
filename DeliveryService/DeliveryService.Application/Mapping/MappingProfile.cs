using AutoMapper;
using OrderService.Domain.Models;
using DeliveryService.Application.DTOs;

namespace OrderService.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Order, DeliveryRequestViewModel>();
            CreateMap<CreateDeliveryRequestDTO, Order>();
            CreateMap<UpdateDeliveryRequestDTO, Order>();
        }
    }
}