using Sender;
using Sender.Messaging;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<RabbitMQSendMessageDirectlyToQueue>();
builder.Services.AddSingleton<RabbitMQSendMessageToExchange>();

var app = builder.Build();

IRabbitMQSender sender;

while (true)
{
    Console.WriteLine("Do you want to send the message to a Queue or an Exchange? (Enter Q for Queue, E for Exchange, or press Enter to exit):");
    string choice = Console.ReadLine();

    if (string.IsNullOrEmpty(choice))
    {
        break;
    }

    if (choice.Equals("Q", StringComparison.OrdinalIgnoreCase))
    {
        sender = app.Services.GetRequiredService<RabbitMQSendMessageDirectlyToQueue>();
    }
    else if (choice.Equals("E", StringComparison.OrdinalIgnoreCase))
    {
        sender = app.Services.GetRequiredService<RabbitMQSendMessageToExchange>();
    }
    else
    {
        Console.WriteLine("Invalid choice. Please enter 'Q' for Queue or 'E' for Exchange.");
        continue;
    }

    Console.WriteLine("Enter the message you want to send (or press Enter to exit):");
    string message = Console.ReadLine();

    if (string.IsNullOrEmpty(message))
    {
        break;
    }

    sender.Send(message);
    Console.WriteLine("Message sent!\n");
}

app.Run();


