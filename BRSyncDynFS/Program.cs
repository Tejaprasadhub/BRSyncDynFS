using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.ComponentModel.Design;
using System.Threading.Tasks;
using BRSyncDynFS.Interfaces;
using BRSyncDynFS.Services;

class Program
{
    static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton<IConfiguration>(context.Configuration);
                // Register service with interface
                services.AddSingleton<IBRPublisher, BRPublisher>();
                services.AddSingleton<IBRReceiver, BRReceiver>();
                services.AddHttpClient<ILogin, Login>();
            })
            .Build();

        // Resolve the services using the interfaces
        var service = host.Services.GetRequiredService<IBRPublisher>();
        var receiver = host.Services.GetRequiredService<IBRReceiver>();
        var login = host.Services.GetRequiredService<ILogin>();

        // User prompt
        Console.WriteLine("Enter command (1 = Publish Message, 2 = Receive Message, 3 = Login): ");
        var input = Console.ReadLine();
        switch (input)
        {
            case "1":
                await service.PublishMessageAsync("Hello from Azure Service Bus!");
                break;
            case "2":
                await receiver.ReceiveMessagesAsync();
                break;
            case "3":
                await login.PerformLogin();
                break;
            default:
                await service.PublishMessageAsync("Hello from Azure Service Bus!");
                break;
        }
    }
}