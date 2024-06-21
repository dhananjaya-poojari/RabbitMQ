using MassTransit;
using Common.Data;
using Sender.RabbitMQ;
using Common.Client;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// In-Memory Database
builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseInMemoryDatabase("CommonInMemoryDb"));

builder.Services.AddSingleton<RabbitMQSendMessageDirectlyToQueue>();
builder.Services.AddSingleton<RabbitMQSendMessageToExchange>();

builder.Services.AddMassTransit(configure =>
{
    configure.SetKebabCaseEndpointNameFormatter();

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

builder.Services.AddHttpClient<StockClient>(httpClient =>
{
    httpClient.BaseAddress = new Uri("https://www.alphavantage.co/query");
    //httpClient.DefaultRequestHeaders.Add("apikey", "UOB842P4MLGQKO10");
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();


