using Bookstore.Domain.Enums;

namespace Bookstore.Application.DTOs.Order;

public sealed record GetOrderDto(Guid OrderId, OrderStatus OrderStatus, decimal TotalPrice, List<string> BookISBNList);