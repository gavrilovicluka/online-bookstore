using Bookstore.Application.DTOs.Order;
using Bookstore.Application.Orders.Commands.CreateOrder;
using Bookstore.Application.Orders.Commands.UpdateOrderStatus;
using Bookstore.Application.Orders.Queries.GetOrderById;
using Bookstore.Application.Orders.Queries.GetOrderByUserId;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Api.Controllers;


public class OrderController : ControllerBase
{
    public OrderController(IMediator mediator) : base(mediator)
    {
    }


    /// <summary>
    /// Get all user's orders
    /// </summary>
    /// <returns> List of orders </returns>
    [HttpGet("User")]
    public async Task<ActionResult> GetOrdersByUserId()
    {
        var response = await _mediator.Send(new GetOrderByUserIdQuery());
        return Ok(response);
    }
    
    /// <summary>
    /// Get order with given ID
    /// </summary>
    /// <param name="orderId"> ID of order </param>
    /// <returns> One order </returns>
    [HttpGet("{orderId:guid}")]
    public async Task<ActionResult> GetOrderById(Guid orderId)
    {
        var response = await _mediator.Send(new GetOrderByIdQuery(orderId));
        return Ok(response);
    }
    
    /// <summary>
    /// Creates new order with books for user
    /// </summary>
    /// <param name="newOrder"> List of books' ISBN and paid status </param>
    /// <returns> Total price and paid status </returns>
    [HttpPost]
    public async Task<ActionResult> CreateOrder([FromBody] CreateOrderDto newOrder)
    {    
        var response = await _mediator.Send(new CreateOrderCommand(newOrder));
        return Ok(response);
    }

    /// <summary>
    /// Updates order status from unpaid to paid
    /// </summary>
    /// <returns> Success message </returns>
    [HttpPut("{orderId:guid}")]
    public async Task<ActionResult> UpdateOrderStatus([FromRoute] Guid orderId)
    {  
        await _mediator.Send(new UpdateOrderStatusCommand(orderId));
        return Ok("Order successfully paid.");
    }
}