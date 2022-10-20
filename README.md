# Demo-Digimon-IoT

![](https://github.com/luisllamasbinaburo/Demo-Digimon-IoT/raw/main/Images/ll-reto-digimon.png)

Ficheros de la Demostración de IoT, aprovechando el Reto de programación de Digimon

El objetivo es leer una tarjeta NFC con un móvil con Android, y mostrar la imagen del Digimon correspondiente en dos dispositivos ESP32, un M5 Stack Core 2 con pantalla TFT, y un M5 Paper con pantalla de tinta electrónica

Para ello se usara un API Rest intermedio, que aceptara llamadas HTTP del móvil Android. La comunicación entre el servidor y los dispositivos finales ESP32 se realizara por MQTT

Las imágenes de los Digimon se obtendran a través del API Rest público https://www.digi-api.com/

## Esquema
La solución consta de,

- Aplicación para Android escrita en Xamarin (C#). Realiza la lectura del NFC, y manda el codigo leido al backend por HTTP. Os dejo comentada la parte de enviar en MQTT por si os es útil para otro proyecto
- Aplicacion en AspNet Core 6, que levanta un Api REST sencilla, que acepta peticiones POST en el endpoint /item, con body formateado en JSON. Esta, convierte el ID recibido en la URL de la imagen, y la envia por MQTT
- Código para ESP32, que recibe una URL por MQTT, y carga la imagen en la pantalla.
![](https://github.com/luisllamasbinaburo/Demo-Digimon-IoT/raw/main/Images/esquema.png)

Lógicamente es una Demo para ilustrar el uso de distintas tecnologias, sobre todo en cuanto a la comunicación. En un proyecto real faltarian muchas cosas. 

Por ejemplo, el Api REST que hemos simulado sería mucho más complejo (más acciones, contendria sus propios ficheros, leeria de una base de datos, etc), habría que securizar todo, etc

## Resultado

Aquí tenéis el resultado, ejecutandose en el M5 Stack Core 2
![](https://github.com/luisllamasbinaburo/Demo-Digimon-IoT/raw/main/Images/m5stack-core-2.jpg)

Y en el M5Paper
![](https://github.com/luisllamasbinaburo/Demo-Digimon-IoT/raw/main/Images/m5paper.jpg)

