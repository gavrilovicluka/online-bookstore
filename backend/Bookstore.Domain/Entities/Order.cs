using Bookstore.Domain.Enums;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.EntityFrameworkCore;

namespace Bookstore.Domain.Entities;

[Collection("orders")]
public class Order
{
    [BsonId(IdGenerator = typeof(CombGuidGenerator))]
    public Guid Id { get; set; }

    [BsonElement("total_price")]
    public decimal TotalPrice { get; set; }
    
    [BsonElement("order_status")]
    public OrderStatus OrderStatus { get; set; }
    
    [BsonElement("user_id")]
    public Guid UserId { get; set; }

    [BsonElement("books_ids")] 
    public List<string> BooksISBNList { get; set; } = new List<string>();
}