using Bookstore.Application.CQRS.Books.Queries.GetBookByISBN;

namespace Bookstore.Application.DTOs.Order;

public sealed record CreateOrderDto(List<GetBookByISBNQuery> BookISBNList, bool IsPaid);