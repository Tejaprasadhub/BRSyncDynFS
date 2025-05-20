using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BRSyncDynFS.Interfaces;

namespace BRSyncDynFS.Services
{
    public class BRPublisher : IBRPublisher
    {
        private readonly IConfiguration _configuration;

        public BRPublisher(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task PublishMessageAsync(string messageBody)
        {
            string? ServiceBusConnectionString = _configuration["AppSettings:ServiceBusConnectionString"];
            string? QueueName = _configuration["AppSettings:QueueName"];

            if (string.IsNullOrEmpty(ServiceBusConnectionString) || string.IsNullOrEmpty(QueueName))
            {
                throw new InvalidOperationException("ServiceBusConnectionString or QueueName is not configured properly.");
            }
            await using var client = new ServiceBusClient(ServiceBusConnectionString);
            ServiceBusSender sender = client.CreateSender(QueueName);

            ServiceBusMessage message = new ServiceBusMessage(messageBody);

            await sender.SendMessageAsync(message);
            Console.WriteLine($"Sent message: {messageBody}");
        }

    }
}
