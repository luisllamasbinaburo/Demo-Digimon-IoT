using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

MqttService mqttService = new MqttService();
await mqttService.Initialize("192.168.1.xxx");

var app = builder.Build();
app.Urls.Add("http://*:3000");

app.MapPost("/item", async (Dto body) =>
{
    Console.WriteLine(body.data);

    var mqtt_payload = NfcCodeToDigimonUrl(body.data);
    await mqttService.SendMqtt(mqtt_payload);

    return new { message = $"Item {mqtt_payload}" };
})
.WithName("Post_Item");

app.Run();

static string NfcCodeToDigimonUrl(string code)
{
    var result = "";
    if (code == "735605776") result = GetDigimonImageUrl("Guilmon");
    if (code == "1508327112") result = GetDigimonImageUrl("DORUgamon");

    return result;
}

static string GetDigimonImageUrl(string digimon_name)
{
    return @$"https://digimon-api.com/images/digimon/w/{digimon_name}.png";
}