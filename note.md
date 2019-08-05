github desktop config
```
C:\Users\mcong\AppData\Local\GitHubDesktop\app-0.8.0\resources\app\git\cmd>git.e
xe config --global http.proxy http://135.245.48.34:8000

C:\Users\mcong\AppData\Local\GitHubDesktop\app-0.8.0\resources\app\git\cmd>git.e
xe config --global https.proxy http://135.245.48.34:8000

git config --global --unset http.proxy
git config --global --unset https.proxy
```
CMake find package
```
/usr/bin/ld: ../libs/cpprestsdk/build.release/Binaries/libcpprest.a(http_server_asio.cpp.o): undefined reference to symbol 'pthread_rwlock_wrlock@@GLIBC_2.2.5'
/usr/lib64/libpthread.so.0: error adding symbols: DSO missing from command line


Add the right find_package invokation to your CMakeLists.txt:

find_package(Threads)

Then, link the library to your target:

target_link_libraries(my_target ${CMAKE_THREAD_LIBS_INIT})

That's all. Likely you forgot the target_link_libraries.
```
curl
```
curl -X GET --user savagecm@qq.com:123456 -v  http://135.251.166.251:6502/v1/ivmero/api/users/signon
curl -X POST -d '{"email":"savagecm@qq.com","password":"123456","name":"Max","lastName":"Cong"}' -v -H "Content-Ty"  http://135.251.166.251:6502/v1/ivmero/api/users/signup
```
# docker
## pull image speedup
```
docker pull registry.docker-cn.com/library/ubuntu:16.04
```
## how to find file in the container using container id
docker inspect --format='{{json .GraphDriver.Data.MergedDir}}' + container id
http client/server benchmark
RESULTS
bench\res 	req/sec 	compared to pure-asio impl
pure-asio 	517737.34 	100%
Pistache 	163593.97 	31.60%
RESTinio (no timers) 	152683.06 	29.49%
Beast 	145949.15 	28.19%
RESTinio (timers) 	115309.33 	22.27%
Restbed 	58223.26 	11.25%
Cpprestsdk 	9487.37 	1.83%



 while true;do test=`ps -ef | grep -v "grep" |grep "redis-server 135.251.166.251"|awk '{print $2}'`;echo $test;`sudo kill -9 $test`;sleep 1;done
## how to find image && image list
 curl <registry URL>/v2/<name dir>/tags/list
## how to find image rep
 curl <registry URL>/v2/_catalog
