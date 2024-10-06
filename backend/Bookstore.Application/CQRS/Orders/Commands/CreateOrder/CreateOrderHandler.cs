using Bookstore.Application.DTOs.Order;
using Bookstore.Application.Helpers;
using Bookstore.Domain.Contracts;
using Bookstore.Domain.Entities;
using Bookstore.Domain.Enums;
using Bookstore.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Bookstore.Application.Orders.Commands.CreateOrder;

public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, CreateOrderResponseDto>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IBookRepository _bookRepository;
    private readonly UserManager<User> _userManager;

    public CreateOrderHandler(IOrderRepository orderRepository, IBookRepository bookRepository, UserManager<User> userManager)
    {
        _orderRepository = orderRepository;
        _bookRepository = bookRepository;
        _userManager = userManager;
    }
    
    public async Task<CreateOrderResponseDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var userId = UserHelper.GetUserIdFromClaim(new HttpContextAccessor());
        if (userId == null)
        {
            throw new BadRequestException($"User does not exist.");
        }
        
        var user = await _userManager.GetUserAsync(UserHelper.GetUserClaimsPrincipal(new HttpContextAccessor()));
        
        if (user == null)
        {
            throw new BadRequestException("User not found.");
        }

        decimal totalPrice = 0;
        foreach (var bookISBN in request.NewOrder.BookISBNList)
        {
            var existingBook = await _bookRepository.GetBookByISBN(bookISBN.ISBN);
            if (existingBook == null)
            {
                throw new BadRequestException($"Book with ISBN: {bookISBN} does not exist.");
            }

            totalPrice += existingBook.Price;
        }

        var newOrder = new Order
        {
            BooksISBNList = request.NewOrder.BookISBNList.Select(x => x.ISBN).ToList(),
            UserId = Guid.Parse(userId),
            TotalPrice = totalPrice,
            OrderStatus = request.NewOrder.IsPaid ? OrderStatus.Paid : OrderStatus.Unpaid
        };
        
        var orderId = _orderRepository.CreateOrder(newOrder);
        
        user.OrdersIds.Add(orderId);
        foreach (var ISBN in request.NewOrder.BookISBNList)
        {
            user.BooksISBN.Add(ISBN.ISBN);
        }
        await _userManager.UpdateAsync(user);

        return new CreateOrderResponseDto(totalPrice, newOrder.OrderStatus);
    }
}