github desktop config
```
C:\Users\mcong\AppData\Local\GitHubDesktop\app-0.8.0\resources\app\git\cmd>git.e
xe config --global http.proxy http://135.245.48.34:8000

C:\Users\mcong\AppData\Local\GitHubDesktop\app-0.8.0\resources\app\git\cmd>git.e
xe config --global https.proxy http://135.245.48.34:8000
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