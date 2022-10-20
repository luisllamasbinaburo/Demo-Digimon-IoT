using MQTTnet.Client;
using MQTTnet;
using System.Text;

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
                                   .WithTopic("Demo/Digimon/Requested")
                                   .WithPayload(payload)
                                   .Build();

        await Task.Run(() => mqttClient.PublishAsync(applicationMessage));
    }
}