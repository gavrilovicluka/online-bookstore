using Bookstore.Domain.Entities;

namespace Bookstore.Domain.Contracts;

public interface IOrderRepository
{
    Order? GetOrder(Guid orderId);
    IEnumerable<Order> GetOrdersByUserId(Guid userId);
    Guid CreateOrder(Order order);
    void UpdateOrder(Order order);
}