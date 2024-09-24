using Ecommerce.Domain.src.Entities.OrderAggregate;
using Ecommerce.Service.src.Shared;

namespace Ecommerce.Service.src.OrderItemService
{
    public interface IOrderItemManagement : IBaseService<OrderItem, OrderItemReadDto, OrderItemCreateDto, OrderItemUpdateDto>
    {
        Task<IEnumerable<OrderItemReadDto>> GetOrderItemsByOrderIdAsync(Guid orderId);
        Task<IEnumerable<OrderItemReadDto>> GetOrderItemsByProductIdAsync(Guid productId);
        Task<int> GetTotalQuantityByOrderIdAsync(Guid orderId);
    }
}