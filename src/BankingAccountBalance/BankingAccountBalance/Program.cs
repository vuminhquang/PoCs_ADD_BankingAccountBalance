using BusinessApplication;
using Contract;
using Domain;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Configure DbContext with in-memory database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseInMemoryDatabase("InMemoryDb");
});

// Register repositories
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

// Register services
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ITransactionProcessor, TransactionProcessor>();

// Redis configuration for background service & for locking mechanism
builder.Services.Configure<RedisConfiguration>(builder.Configuration.GetSection("Redis"));
// Redis connection
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = sp.GetRequiredService<IOptions<RedisConfiguration>>().Value;
    return ConnectionMultiplexer.Connect(configuration.ConnectionString);
});
builder.Services.AddHostedService<RedisBackgroundService>();

var app = builder.Build();

// Define endpoints
app.MapPost("/process-transaction", async (TransactionDto transactionDto, ITransactionProcessor processor) =>
{
    await processor.ProcessTransactionAsync(transactionDto);
    return Results.Ok("Transaction processed successfully");
});


app.Run();