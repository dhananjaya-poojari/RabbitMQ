using Reciever;

var builder = WebApplication.CreateBuilder(args);

// Register RabbitMQ queue receiver as a hosted service
builder.Services.AddHostedService<RabbitMQQueueReciever>();

// Register RabbitMQ exchange receiver as a singleton service
builder.Services.AddSingleton<RabbitMQExchangeReciever>();

var app = builder.Build();

// Configure RabbitMQ receiver to start and stop with the application
app.UseRabbitMQReciever();

// Run the application
app.Run();
