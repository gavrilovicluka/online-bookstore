using Bookstore.Application.DTOs.Order;
using Bookstore.Application.Helpers;
using Bookstore.Domain.Contracts;
using Bookstore.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Bookstore.Application.Orders.Queries.GetOrderByUserId;

public class GetOrderByUserIdHandler : IRequestHandler<GetOrderByUserIdQuery, IEnumerable<GetOrderDto>>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderByUserIdHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    
    public Task<IEnumerable<GetOrderDto>> Handle(GetOrderByUserIdQuery request, CancellationToken cancellationToken)
    {
        var userId = UserHelper.GetUserIdFromClaim(new HttpContextAccessor());

        if (userId == null)
        {
            throw new BadRequestException("User ID not found.");
        }        
        
        var orders = _orderRepository.GetOrdersByUserId(Guid.Parse(userId));

        var orderDtos = orders.Select(o => new GetOrderDto(o.Id, o.OrderStatus, o.TotalPrice, o.BooksISBNList));

        return Task.FromResult(orderDtos);
    }
}