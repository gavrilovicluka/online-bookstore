using Bookstore.Application.DTOs.Order;
using Bookstore.Domain.Contracts;
using Bookstore.Domain.Exceptions;
using MediatR;

namespace Bookstore.Application.Orders.Queries.GetOrderById;

public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, GetOrderDto>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderByIdHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    
    public Task<GetOrderDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = _orderRepository.GetOrder(request.OrderId);

        if (order == null)
        {
            throw new BadRequestException($"Order with ID: {request.OrderId} does not exist.");
        }

        var orderDto = new GetOrderDto(order.Id, order.OrderStatus, order.TotalPrice, order.BooksISBNList);

        return Task.FromResult(orderDto);
    }
}