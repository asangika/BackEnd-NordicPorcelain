namespace Ecommerce.Domain.Enums
{
    public enum ShippingStatus
    {
        NotShipped,
        PreparingForShipment,
        Shipped,
        InTransit,
        OutForDelivery,
        Delivered,
        DeliveryAttempted,
        ReturnedToSender,
        LostInTransit,
        HeldAtCustoms,
        Canceled
    }
}