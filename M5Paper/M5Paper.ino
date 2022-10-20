#include <M5EPD.h>

#include <WiFi.h>
#include <ESPAsyncWebServer.h>
#include <FS.h>
#include <PubSubClient.h>

#include "./config.h"

#include "./src/M5StackUtils.h"
#include "./src/MqttService.hpp";

MqttService mqttService;
M5EPD_Canvas canvas(&M5.EPD);

void setup()
{
	Serial.begin(115200);

	M5.begin();

	M5.EPD.Clear(true);
	canvas.createCanvas(960, 540);
	canvas.setTextSize(3);

	WIFI_Connect();

	delay(2000);

	mqttService.RecieveCallback = [](String image_url)
	{
		Serial.println(image_url);
		if (image_url == "")
		{
			M5.EPD.Clear(true);
		}
		else
		{
			canvas.drawPngUrl(image_url.c_str());
			canvas.pushCanvas(0, 0, UPDATE_MODE_GC16);
		}
	};
	mqttService.Init();

	delay(1500);
}

void loop()
{
	M5.update();

	mqttService.HandleMqtt();

	delay(10);
}
