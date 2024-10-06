using System.Net;

namespace Bookstore.Domain.Exceptions;

public record ExceptionResponse(HttpStatusCode StatusCode, string Description);