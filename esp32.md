# macos
## check port
ls /dev/cu.*
/dev/cu.usbserial-02133DB2
## source env
. $HOME/esp/esp-idf/export.sh
## getting started
https://docs.espressif.com/projects/esp-idf/zh_CN/latest/esp32/get-started/index.html#get-started-get-esp-idf
## steps
### build
idf.py build
### flash
idf.py -p PORT [-b BAUD] flash
example:
idf.py -p /dev/cu.usbserial-02133DB2 flash
## monitor
idf.py -p PORT monitor 

## set target board and config project
idf.py set-target esp32
idf.py menuconfig

## components size
make size-components 查看生成的固件内存使用情况
