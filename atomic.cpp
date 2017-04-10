#include <iostream>       // std::cout
#include <atomic>         // std::atomic
#include <thread>         // std::thread
#include <vector>         // std::vector
#define MAX_INT    (((unsigned int)(-1))>>1)
std::atomic<int> stomic_test(MAX_INT-3);
void change_atomic (int val) {
        for (int i=0; i<1;i++)
        {
                std::cout<<stomic_test++<<std::endl;

        }


}
int main ()
{
  // spawn 10 threads to test
  std::vector<std::thread> threads;
  for (int i=0; i<10; ++i) threads.push_back(std::thread(change_atomic,i));
  for (auto& th : threads) th.join();
  return 0;
}
