using Bookstore.Api;
using Bookstore.Api.Middlewares;
using Bookstore.Application;
using Bookstore.Application.JwtFeatures;
using Bookstore.Persistence;
using NLog.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// For logging in file: D:\OnlineBookstore\OnlineBookstore\Bookstore.Api\bin\Debug\net8.0\logs
// builder.Services.AddLogging(logging =>
// {
//     logging.ClearProviders();
//     logging.SetMinimumLevel(LogLevel.Trace);
// });
builder.Services.AddSingleton<ILoggerProvider, NLogLoggerProvider>();

builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddApplicationServices();

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
builder.Services.AddDocSwagger();


builder.Services.AddJwtAuthentication(builder.Configuration);
// .AddBearerToken(IdentityConstants.BearerScheme);     // Za builder.Services.AddAuthentication()
builder.Services.AddSingleton<JwtHandler>();

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers().RequireAuthorization();

// app.MapIdentityApi<User>();

app.Run();