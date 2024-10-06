using System.ComponentModel.DataAnnotations;
using Amazon.DynamoDBv2.DataModel;
using Bookstore.Domain.Enums;

namespace Bookstore.Domain.Entities;

[DynamoDBTable("Books")]
public class Book
{
    [DynamoDBHashKey]
    [Required]
    public string? ISBN { get; init; }

    [DynamoDBProperty]
    public string? Title { get; set; }
    
    [DynamoDBProperty]
    public string? Author { get; set; }
    
    [DynamoDBProperty]
    public string? Category { get; set; }
    
    [DynamoDBProperty]
    public decimal Price { get; set; }
    
    [DynamoDBProperty]
    public AvailabilityStatus Available { get; set; }

    [DynamoDBProperty] 
    public List<Guid> ReviewIds { get; set; } = new List<Guid>();
}