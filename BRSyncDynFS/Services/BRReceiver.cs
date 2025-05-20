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
    public class BRReceiver : IBRReceiver
    {
        private readonly IConfiguration _configuration;

        public BRReceiver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public  async Task ReceiveMessagesAsync()
        {
            string? ServiceBusConnectionString = _configuration["AppSettings:ServiceBusConnectionString"];
            string? QueueName = _configuration["AppSettings:QueueName"];

            if (string.IsNullOrEmpty(ServiceBusConnectionString) || string.IsNullOrEmpty(QueueName))
            {
                throw new InvalidOperationException("ServiceBusConnectionString or QueueName is not configured properly.");
            }

            await using var client = new ServiceBusClient(ServiceBusConnectionString);
            ServiceBusReceiver receiver = client.CreateReceiver(QueueName);

            ServiceBusReceivedMessage receivedMessage = await receiver.ReceiveMessageAsync();
            if (receivedMessage != null)
            {
                string body = receivedMessage.Body.ToString();
                Console.WriteLine($"Received message: {body}");

                await receiver.CompleteMessageAsync(receivedMessage);
            }
            else
            {
                Console.WriteLine("No messages available to receive.");
            }
        }
    }
}
