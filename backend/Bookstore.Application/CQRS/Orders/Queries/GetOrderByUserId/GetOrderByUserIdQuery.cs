using Bookstore.Application.DTOs.Order;
using MediatR;

namespace Bookstore.Application.Orders.Queries.GetOrderByUserId;

public sealed record GetOrderByUserIdQuery() : IRequest<IEnumerable<GetOrderDto>>;