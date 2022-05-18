using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsurancePolicies.API.Services
{
    public class MessagePublisher : IMessagePublisher
    {
        private const string _queueName = "insurancepolicyqueue";
        private readonly ServiceBusClient _serviceBusClient;
        private readonly ServiceBusSender _serviceBusSender;

        public MessagePublisher(ServiceBusClient serviceBusClient)
        {
            _serviceBusClient = serviceBusClient;
            _serviceBusSender = _serviceBusClient.CreateSender(_queueName);
        }

        public async Task Publish<T>(T obj)
        {
            await Task.Delay(10000);
            var raw = JsonConvert.SerializeObject(obj);
            var message = new ServiceBusMessage(Encoding.UTF8.GetBytes(raw));
            await _serviceBusSender.SendMessageAsync(message);
        }
    }
}
