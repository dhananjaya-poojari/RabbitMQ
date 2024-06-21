using Common.Client;
using Common.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Reciever;
using Sender.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

// In-Memory Database
builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseInMemoryDatabase("CommonInMemoryDb"));

// 3rd party Client Configuration
builder.Services.AddHttpClient<StockClient>(httpClient =>
{
    httpClient.BaseAddress = new Uri("https://www.alphavantage.co/query");
    //httpClient.DefaultRequestHeaders.Add("apikey", "UOB842P4MLGQKO10");
});

builder.Services.AddMassTransit(configure =>
{
    configure.SetKebabCaseEndpointNameFormatter();

    configure.AddConsumer<PurchaseOrderSentConsumer>();

    configure.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(new Uri("amqp://localhost"), (h) =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        cfg.ConfigureEndpoints(context);
    });
});

// Register RabbitMQ queue receiver as a hosted service
builder.Services.AddHostedService<RabbitMQQueueReciever>();

// Register RabbitMQ exchange receiver as a singleton service
builder.Services.AddSingleton<RabbitMQExchangeReciever>();


var app = builder.Build();

// Configure RabbitMQ receiver to start and stop with the application
app.UseRabbitMQReciever();

// Run the application
app.Run();
