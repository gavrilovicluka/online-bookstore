using Bookstore.Domain.Enums;

namespace Bookstore.Application.DTOs.Book;

public record UpdateBookDto(string? Title, string? Author, string? Category, decimal Price, AvailabilityStatus Available);