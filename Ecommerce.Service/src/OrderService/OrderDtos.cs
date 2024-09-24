using Ecommerce.Domain.Enums;
using Ecommerce.Domain.src.Entities.OrderAggregate;
using Ecommerce.Service.src.OrderItemService;
using Ecommerce.Service.src.Shared;

namespace Ecommerce.Service.src.OrderService
{
    public class OrderReadDto : BaseReadDto<Order>
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public Guid ShippingAddressId { get; set; }
        public OrderStatus OrderStatus { get; set; }

        public override void FromEntity(Order entity)
        {
            OrderId = entity.Id;
            UserId = entity.UserId;
            OrderDate = entity.OrderDate;
            TotalPrice = entity.TotalPrice;
            ShippingAddressId = entity.ShippingAddressId;
            OrderStatus = entity.OrderStatus;
            base.FromEntity(entity);
        }
    }
    public class OrderCreateDto : ICreateDto<Order>
    {
        public Guid UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public Guid ShippingAddressId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public IEnumerable<OrderItemCreateDto> OrderItems { get; set; }

        public Order CreateEntity()
        {
            return new Order
            {
                UserId = UserId,
                OrderDate = OrderDate,
                TotalPrice = TotalPrice,
                ShippingAddressId = ShippingAddressId,
                OrderStatus = OrderStatus,
                OrderItems = OrderItems.Select(item => item.CreateEntity()).ToList()
            };
        }
    }
    public class OrderUpdateDto : IUpdateDto<Order>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public Guid ShippingAddressId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public IEnumerable<OrderItemUpdateDto> OrderItems { get; set; }

        public Order UpdateEntity(Order entity)
        {
            entity.TotalPrice = TotalPrice;
            entity.ShippingAddressId = ShippingAddressId;
            entity.OrderStatus = OrderStatus;
            entity.OrderItems = OrderItems.Select(item => item.UpdateEntity(entity.OrderItems.FirstOrDefault(oi => oi.Id == item.Id))).ToList();
            return entity;
        }
    }
}