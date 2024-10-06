using Bookstore.Domain.Enums;

namespace Bookstore.Application.DTOs.Order;

public sealed record CreateOrderResponseDto(decimal TotalPrice, OrderStatus Status);