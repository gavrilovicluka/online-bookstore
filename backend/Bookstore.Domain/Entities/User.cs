using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.EntityFrameworkCore;

namespace Bookstore.Domain.Entities;

[Collection("Users")]
public class User : MongoIdentityUser<Guid>
{
    [BsonElement("first_name")]
    public string? FirstName { get; set; }
 
    [BsonElement("last_name")]
    public string? LastName { get; set; }

    [BsonElement("orders_ids")] 
    public List<Guid> OrdersIds { get; set; } = new List<Guid>();
    
    [BsonElement("reviews_ids")]
    public List<Guid> ReviewsIds { get; set; } = new List<Guid>();
    
    [BsonElement("books_isbn")]
    public List<string> BooksISBN { get; set; } = new List<string>();
}