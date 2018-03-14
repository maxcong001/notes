## docker run --rm --cap-add SYS_RAWIO --device /dev/mem hypriot/rpi-gpio readall
## http://harttle.land/2017/01/14/raspberrypi-uart.html

## docker priviledge
https://docs.docker.com/engine/reference/run/#runtime-privilege-and-linux-capabilities
## sudo docker run -ti --privileged=true   max/lora
