#include <M5Core2.h>

#include <WiFi.h>
#include <HTTPClient.h>
#include <PubSubClient.h>

#include "./config.h"

#include "./src/M5StackUtils.h"
#include "./src/MqttService.hpp";

MqttService mqttService;

void setup() {
	Serial.begin(115200);

	M5.begin(true, true, true, false);

	WIFI_Connect();

	delay(2000);

	mqttService.RecieveCallback = [](String image_url)
	{
		if (image_url == "")
		{
			M5.Lcd.fillScreen(TFT_BLACK);
		}
		else
		{
			M5.Lcd.drawPngUrl(image_url.c_str());
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