using Bookstore.Application.DTOs.Order;
using MediatR;

namespace Bookstore.Application.Orders.Commands.CreateOrder;

public sealed record CreateOrderCommand(CreateOrderDto NewOrder) : IRequest<CreateOrderResponseDto>;