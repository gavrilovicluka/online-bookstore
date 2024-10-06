using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.EntityFrameworkCore;

namespace Bookstore.Domain.Entities;

[Collection("reviews")]
public class Review
{
    [BsonId(IdGenerator = typeof(CombGuidGenerator))]
    public Guid Id { get; set; }
    
    [BsonElement("review_text")]
    [MaxLength(100)]
    public string? ReviewText { get; set; }
    
    [BsonElement("rating")]
    [Range(1, 5)]
    public int Rating { get; set; }
    
    [BsonElement("book_isbn")]
    public string? BookISBN { get; set; }
    
    [BsonElement("user_id")]
    public Guid UserId { get; set; }
}