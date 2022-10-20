using MQTTnet;
using MQTTnet.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NFCAndroid
{
    internal class MqttService
    {
        private IMqttClient mqttClient;

        public async Task Initialize(string brokerIpAddress)
        {
            var factory = new MqttFactory();

            var options = new MqttClientOptionsBuilder()
            .WithClientId("Client1")
            .WithTcpServer(brokerIpAddress, 1883)
            .Build();

            mqttClient = factory.CreateMqttClient();

            mqttClient.ConnectedAsync += (async e =>
            {
                await mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("hello/world").Build());
            });

            mqttClient.ApplicationMessageReceivedAsync += (async e =>
            {
                Console.WriteLine($"+ Payload = {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
            });

            await mqttClient.ConnectAsync(options, CancellationToken.None);
        }

        public async Task SendMqtt(string payload)
        {
            var applicationMessage = new MqttApplicationMessageBuilder()
                                       .WithTopic("Demo/Device1/Received")
                                       .WithPayload(payload)
                                       .Build();

            await Task.Run(() => mqttClient.PublishAsync(applicationMessage));
        }
    }
}
