using Bookstore.Domain.Contracts;
using Bookstore.Domain.Enums;
using Bookstore.Domain.Exceptions;
using MediatR;

namespace Bookstore.Application.Orders.Commands.UpdateOrderStatus;

public class UpdateOrderStatusHandler : IRequestHandler<UpdateOrderStatusCommand, Unit>
{
    private readonly IOrderRepository _orderRepository;

    public UpdateOrderStatusHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    
    public Task<Unit> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = _orderRepository.GetOrder(request.OrderId);

        if (order == null)
        {
            throw new BadRequestException($"Order with ID: {request.OrderId} does not exist.");
        }

        order.OrderStatus = OrderStatus.Paid;
        
        _orderRepository.UpdateOrder(order);

        return Unit.Task;
    }
}