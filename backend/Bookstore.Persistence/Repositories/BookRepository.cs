using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Bookstore.Domain.Entities;
using Bookstore.Domain.Enums;
using Microsoft.Extensions.Logging;
using IBookRepository = Bookstore.Domain.Contracts.IBookRepository;

namespace Bookstore.Persistence.Repositories;

public class BookRepository : IBookRepository
{
    private readonly IAmazonDynamoDB _amazonDynamoDb;
    private readonly string tableName = "booksTable";
    private readonly ILogger<BookRepository> _logger;

    public BookRepository(IAmazonDynamoDB amazonDynammoDb, ILogger<BookRepository> logger)
    {
        _amazonDynamoDb = amazonDynammoDb;
        _logger = logger;
        
        var result= CreateTableAsync(BooksItemsAttributes, BooksKeySchema, BooksTableProvisionedThroughput);

        if (!result.Result)
        {
            _logger.LogError("Could not create table.");
        }
    }

    public async Task<IEnumerable<Book>> GetAllBooks()
    {
        List<Book> books = [];
        
        var request = new ScanRequest
        {
            TableName = tableName
        };

        var response = await _amazonDynamoDb.ScanAsync(request);

        foreach (Dictionary<string, AttributeValue> item in response.Items)
        {
            Enum.TryParse(item["Available"].S, out AvailabilityStatus status);

            var reviewIdsList = item["ReviewIds"].SS;
            List<Guid> reviewIdsGuidList = reviewIdsList.Select(x => Guid.Parse(x)).ToList();
            
            books.Add(new Book
            {
                ISBN = item["ISBN"].S,
                Title = item["Title"].S,
                Author = item["Author"].S,
                Category = item["Category"].S,
                Price = decimal.Parse(item["Price"].N),
                Available = status,
                ReviewIds = reviewIdsGuidList
            });
        }

        return books;
    }

    public async Task<Book> GetBookByISBN(string ISBN)
    {
       var request = new GetItemRequest
       {
           TableName = tableName,
           Key = new Dictionary<string, AttributeValue>
           {
               {
                   "ISBN",
                   new AttributeValue
                   {
                       S = ISBN.ToString()
                   }
               }
           }
       };
       var response = await _amazonDynamoDb.GetItemAsync(request);

       if (response.Item.Count == 0)
       {
           return null!;
       }

       Enum.TryParse(response.Item["Available"].S, out AvailabilityStatus status);
       
       var reviewIdsList = response.Item["ReviewIds"].SS;
       List<Guid> reviewIdsGuidList = reviewIdsList.Select(x => Guid.Parse(x)).ToList();
       
       return new Book
       {
           ISBN = response.Item["ISBN"].S,
           Title = response.Item["Title"].S,
           Author = response.Item["Author"].S,
           Category = response.Item["Category"].S,
           Price = decimal.Parse(response.Item["Price"].N),
           Available = status,
           ReviewIds = reviewIdsGuidList
       };
    }
    
    public async Task<List<Book>> GetBooksByTitle(string title)
    {
        var request = new QueryRequest
        {
            TableName = tableName,
            IndexName = "TitleAuthorIndex",
            KeyConditionExpression = "Title = :v_Title",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":v_Title", new AttributeValue{ S = title } }
            }
        };
        var response = await _amazonDynamoDb.QueryAsync(request);

        var booksList = BookDictionaryToList(response.Items);
        return booksList;
    }

    public async Task<List<Book>> GetBooksByAuthor(string author)
    {
        var request = new QueryRequest
        {
            TableName = tableName,
            IndexName = "AuthorCategoryIndex",
            KeyConditionExpression = "Author = :v_Author",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":v_Author", new AttributeValue{ S = author } }
            }
        };
        var response = await _amazonDynamoDb.QueryAsync(request);

        var booksList = BookDictionaryToList(response.Items);
        return booksList;
    }

    public async Task<List<Book>> GetBooksByCategory(string category)
    {
        var request = new QueryRequest
        {
            TableName = tableName,
            IndexName = "CategoryTitleIndex",
            KeyConditionExpression = "Category = :v_Category",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            {
                { ":v_Category", new AttributeValue{ S = category } }
            }
        };
        var response = await _amazonDynamoDb.QueryAsync(request);

        var booksList = BookDictionaryToList(response.Items);
        return booksList;
    }

    public async Task AddBook(Book newBook)
    {
        var request = new PutItemRequest
        {
            TableName = tableName,
            Item = new Dictionary<string, AttributeValue>()
            {
                { "ISBN", new AttributeValue { S = newBook.ISBN }},
                { "Title", new AttributeValue { S = newBook.Title }},
                { "Author", new AttributeValue { S = newBook.Author }},
                { "Price", new AttributeValue { N = newBook.Price.ToString() }},
                { "Category", new AttributeValue { S = newBook.Category }},
                { "Available", new AttributeValue { S = newBook.Available.ToString() }},
                { "ReviewIds", new AttributeValue { NULL = true }}
            }
        };
        
        var response = await _amazonDynamoDb.PutItemAsync(request);
    }
    
    public async Task UpdateBook(Book updateBook)
    {
        var request = new PutItemRequest
        {
            TableName = tableName,
            Item = new Dictionary<string, AttributeValue>()
            {
                { "ISBN", new AttributeValue { S = updateBook.ISBN }},
                { "Title", new AttributeValue { S = updateBook.Title }},
                { "Author", new AttributeValue { S = updateBook.Author }},
                { "Price", new AttributeValue { N = updateBook.Price.ToString() }},
                { "Category", new AttributeValue { S = updateBook.Category }},
                { "Available", new AttributeValue { S = updateBook.Available.ToString() }},
                { "ReviewIds", new AttributeValue { SS = updateBook.ReviewIds.Select(x => x.ToString()).ToList() }}
            }
        };
        
        var response = await _amazonDynamoDb.PutItemAsync(request);
    }
    
    public async Task DeleteBook(string ISBN)
    {
        var request = new DeleteItemRequest
        {
            TableName = tableName,
            Key = new Dictionary<string,AttributeValue>() { { "ISBN", new AttributeValue { S = ISBN } } },
        };

        var response = await _amazonDynamoDb.DeleteItemAsync(request);
    }

    private async Task<bool> CreateTableAsync(
        List<AttributeDefinition> tableAttributes,
        List<KeySchemaElement> tableKeySchema,
        ProvisionedThroughput provisionedThroughput)
    {
        var tables = await _amazonDynamoDb.ListTablesAsync();
        
        bool response = true;
        
        if(!tables.TableNames.Contains(tableName))
        {
            var createTitleAuthorIndex = new GlobalSecondaryIndex
            {
                IndexName = "TitleAuthorIndex",
                KeySchema =
                {
                    new KeySchemaElement
                    {
                        AttributeName = "Title", KeyType = "HASH"
                    },
                    new KeySchemaElement
                    {
                        AttributeName = "Author", KeyType = "RANGE"
                    }
                },
                Projection = new Projection
                {
                    ProjectionType = "ALL"
                },
                ProvisionedThroughput = BooksTableProvisionedThroughput
            };
            
            var createAuthorCategoryIndex = new GlobalSecondaryIndex
            {
                IndexName = "AuthorCategoryIndex",
                KeySchema =
                {
                    new KeySchemaElement
                    {
                        AttributeName = "Author", KeyType = "HASH"
                    },
                    new KeySchemaElement
                    {
                        AttributeName = "Category", KeyType = "RANGE"
                    }
                },
                Projection = new Projection
                {
                    ProjectionType = "ALL"
                },
                ProvisionedThroughput = BooksTableProvisionedThroughput
            };
            
            var createCategoryTitleIndex = new GlobalSecondaryIndex
            {
                IndexName = "CategoryTitleIndex",
                KeySchema =
                {
                    new KeySchemaElement
                    {
                        AttributeName = "Category", KeyType = "HASH"
                    },
                    new KeySchemaElement
                    {
                        AttributeName = "Title", KeyType = "RANGE"
                    }
                },
                Projection = new Projection
                {
                    ProjectionType = "ALL"
                },
                ProvisionedThroughput = BooksTableProvisionedThroughput
            };
            
            var request = new CreateTableRequest
            {
                TableName = tableName,
                KeySchema = tableKeySchema,
                AttributeDefinitions = tableAttributes,
                ProvisionedThroughput = provisionedThroughput,
                GlobalSecondaryIndexes =
                {
                    createTitleAuthorIndex,
                    createAuthorCategoryIndex,
                    createCategoryTitleIndex
                }
            };

            try
            {
                var makeTbl = await _amazonDynamoDb.CreateTableAsync(request);
            }
            catch (Exception e)
            {
                response = false;
                _logger.LogError("Could not create a table: " + e.Message);
            }
        }

        return response;
    }
    
    private static List<AttributeDefinition> BooksItemsAttributes = new List<AttributeDefinition>
    {
        new AttributeDefinition
        {
            AttributeName = "ISBN",
            AttributeType = "S"
        },
        new AttributeDefinition
        {
            AttributeName = "Title",
            AttributeType = "S"
        },
        new AttributeDefinition
        {
            AttributeName = "Author",
            AttributeType = "S"
        },
        new AttributeDefinition
        {
            AttributeName = "Category",
            AttributeType = "S"
        },
    };
    
    private static List<KeySchemaElement> BooksKeySchema = new List<KeySchemaElement>
    {
        new KeySchemaElement
        {
            AttributeName = "ISBN",
            KeyType = "HASH"
        },
    };
    
    private static ProvisionedThroughput BooksTableProvisionedThroughput = new ProvisionedThroughput( 1, 1 );

    private static List<Book> BookDictionaryToList(List<Dictionary<string, AttributeValue>> dict)
    {
        var booksList = new List<Book>();
        foreach (var item in dict)
        {
            Enum.TryParse(item["Available"].S, out AvailabilityStatus status);
       
            var reviewIdsList = item["ReviewIds"].SS;
            List<Guid> reviewIdsGuidList = reviewIdsList.Select(x => Guid.Parse(x)).ToList();

            booksList.Add(new Book
            {
                ISBN = item["ISBN"].S,
                Title = item["Title"].S,
                Author = item["Author"].S,
                Category = item["Category"].S,
                Price = decimal.Parse(item["Price"].N),
                Available = status,
                ReviewIds = reviewIdsGuidList
            });
        }

        return booksList;
    }
}