using Ecommerce.Domain.Enums;
using Ecommerce.Domain.src.Entities.OrderAggregate;

namespace Ecommerce.Service.src.OrderService.Handlers
{
    public interface IOrderStatushandler
    {
        Task HandleOrderStatusAsync(Order order, OrderStatus newStatus);

    }
}