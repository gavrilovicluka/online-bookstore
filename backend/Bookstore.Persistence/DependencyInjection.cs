using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Bookstore.Domain.Contracts;
using Bookstore.Domain.Entities;
using Bookstore.Persistence.Contexts;
using Bookstore.Persistence.Repositories;
using Bookstore.Persistence.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bookstore.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        // DynamoDB setup
        var dynamoDbConfig = configuration.GetSection("DynamoDb");
        var runLocalDynamoDb = dynamoDbConfig.GetValue<bool>("LocalMode");
        var credentials = new BasicAWSCredentials(dynamoDbConfig.GetValue<string>("AccessKey"), dynamoDbConfig.GetValue<string>("SecretKey"));
        services.AddSingleton<IAmazonDynamoDB>(sp =>
        {
            var clientConfig = new AmazonDynamoDBConfig 
            { 
                ServiceURL = dynamoDbConfig.GetValue<string>("LocalServiceUrl") 
            };
            return new AmazonDynamoDBClient(credentials, clientConfig);
        });
        
        
        // MongoDB setup - user identity
        services.AddTransient<IEmailSender<User>, MyEmailSender>();

        var mongoDbSettings = configuration.GetSection(nameof(MongoDbConfig)).Get<MongoDbConfig>();
        services.AddIdentity<User, UserRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireUppercase = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddMongoDbStores<User, UserRole, Guid>
            (
                mongoDbSettings.ConnectionString, mongoDbSettings.Name
            );
            // .AddApiEndpoints();


        // MongoDB setup - EF Core MongoDB for Orders and Reviews 
        services.AddDbContext<MongoDbContext>(options =>
        {
            options.UseMongoDB(mongoDbSettings.ConnectionString, mongoDbSettings.Name);
        });
        
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}

public class MyEmailSender : IEmailSender<User>
{
    public Task SendConfirmationLinkAsync(User user, string email, string confirmationLink)
    {
        return Task.FromResult(true);
    }

    public Task SendPasswordResetLinkAsync(User user, string email, string resetLink)
    {
        return Task.FromResult(true);
    }

    public Task SendPasswordResetCodeAsync(User user, string email, string resetCode)
    {
        return Task.FromResult(true);
    }
};