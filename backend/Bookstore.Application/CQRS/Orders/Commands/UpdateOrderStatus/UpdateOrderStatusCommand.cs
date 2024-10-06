using MediatR;

namespace Bookstore.Application.Orders.Commands.UpdateOrderStatus;

public sealed record UpdateOrderStatusCommand(Guid OrderId) : IRequest<Unit>;