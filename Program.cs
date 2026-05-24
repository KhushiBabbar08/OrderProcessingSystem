using Microsoft.EntityFrameworkCore;
using OrderProcessingSystem.BackgroundServices;
using OrderProcessingSystem.Entities;
using OrderProcessingSystem.Interfaces;
using OrderProcessingSystem.Repositories;
using OrderProcessingSystem.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<OrderProcessingDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = builder.Configuration.GetSection("Redis")
        ["ConnectionString"];

    return ConnectionMultiplexer.Connect(configuration!);
});

builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<IKafkaProducerService, KafkaProducerService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddHostedService<KafkaConsumerService>();

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();