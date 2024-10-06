using Bookstore.Domain.Contracts;
using Bookstore.Domain.Entities;
using Bookstore.Persistence.Contexts;

namespace Bookstore.Persistence.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly MongoDbContext _mongoDbContext;

    public OrderRepository(MongoDbContext mongoDbContext)
    {
        _mongoDbContext = mongoDbContext;
    }

    public Order? GetOrder(Guid orderId)
    {
        return _mongoDbContext.Orders.FirstOrDefault(o => o.Id == orderId);
    }

    public IEnumerable<Order> GetOrdersByUserId(Guid userId)
    {
        return _mongoDbContext.Orders.Where(o => o.UserId == userId).AsEnumerable();
    }

    public Guid CreateOrder(Order order)
    {
        _mongoDbContext.Orders.Add(order);

        _mongoDbContext.SaveChangesAsync();

        return order.Id;
    }

    public void UpdateOrder(Order order)
    {
        _mongoDbContext.Orders.Update(order);

        _mongoDbContext.SaveChangesAsync();
    }
}