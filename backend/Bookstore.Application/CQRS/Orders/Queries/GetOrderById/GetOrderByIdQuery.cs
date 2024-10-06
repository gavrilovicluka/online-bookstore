using Bookstore.Application.DTOs.Order;
using MediatR;

namespace Bookstore.Application.Orders.Queries.GetOrderById;

public sealed record GetOrderByIdQuery(Guid OrderId) : IRequest<GetOrderDto>;