#include <iostream>
#include <utility>
#include <thread>
#include <chrono>
#include <functional>
#include <atomic>
#include <future>
#include <thread>
#include <list>
#include <map>
using namespace std;
typedef std::map<int,int> Map;
std::future<int> f2;
std::promise<void> ready_promise;
std::shared_future<void> ready_future(ready_promise.get_future());
std::promise<Map> fff;
std::shared_future<Map > ff1(fff.get_future());

time_t rawtime;

void thread1()
{
        std::this_thread::sleep_for(std::chrono::milliseconds(5000));
        time (&rawtime);
        std::cout << "before set value " << ctime (&rawtime)<<" \n"<<std::flush;
        Map test1;
        test1[1] = 100;
        fff.set_value(test1);
        time (&rawtime);
        std::cout << "after set value " << ctime (&rawtime)<<" \n"<<std::flush;
        std::this_thread::sleep_for(std::chrono::milliseconds(5000));
        time (&rawtime);
        std::cout << " after sleep for 5 sec , time is : " << ctime (&rawtime)<<" \n"<<std::flush;
}
int main()
{
        time (&rawtime);
        std::cout << "Waiting... time : "<< ctime (&rawtime)<<" \n" << std::flush;

        std::thread t1(thread1);
        std::chrono::milliseconds span (1000);
        while (ff1.wait_for(span)==std::future_status::timeout)
        {
                time (&rawtime);
                std::cout << "Maxx :timeout wating for future "<< ctime (&rawtime)<<" \n"<<std::flush;
                return 0;
        /*
                Map tmp = ff1.get();
                time (&rawtime);
                std::cout << "Done!\tResults are: "<<tmp[1]<<"at time: "<< ctime (&rawtime)<<" \n";
        */

        }
        Map tmp = ff1.get();
        time (&rawtime);
        std::cout << "Done!\tResults are: "<<tmp[1]<<"at time: "<< ctime (&rawtime)<<" \n";

        time (&rawtime);
        std::cout << "timeout wating for future "<<ctime (&rawtime)<<" \n"<<std::flush;

#if 0
        ff1.wait();
        time (&rawtime);
        std::cout << " after wait, time is :  "<<ctime (&rawtime)<<" \n"<<std::flush;
        Map tmp = ff1.get();
        time (&rawtime);
        std::cout << "get result at time  "<<ctime (&rawtime)<<" \n"<<std::flush;
#endif
        t1.join();
}
