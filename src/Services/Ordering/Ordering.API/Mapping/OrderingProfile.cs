using AutoMapper;
using EventBusMessages.Events;
using Ordering.API.EventBusConsumer;
using Ordering.Application.Features.Orders.Commands.ChekoutOrder;

namespace Ordering.API.Mapping
{
    public class OrderingProfile : Profile
    {
        public OrderingProfile()
        {
            CreateMap<CheckoutOrderCommand, BasketCheckoutEvent>().ReverseMap();
        }
    }
}
