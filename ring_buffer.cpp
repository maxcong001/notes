ring


#include <iostream>
#include <deque>
#include <string.h>

#define RB_SIZE 200
#define RB_ELEMENT_SIZE 4096
class ring_buffer
{
    ring_buffer():element_num(0),head_index(0),tail_index(0){};

    int element_num;

    bool init();
    bool destroy();

    bool add(char* buf);
    bool del();
    bool is_empty();

    std::vector<char*> _ring_buffer;

    std::atomic<long> head_index;
    std::atomic<long> tail_index;


};

bool ring_buffer::add(char* buf, size_t length)
{
    if (length >RB_ELEMENT_SIZE)
    {
        return false;
    }

    if (++tail_index == head_index)
    {
        --tail_index;
        return false;
    }

    if (tail_index > RB_SIZE )
    {
        // lock here?
        tail_index = (tail_index + 1) % RB_SIZE;
    }
    char* tmp_queue_ptr = _ring_buffer[tail_index];
    if (!memcpy(tmp_queue_ptr, buf, length))
    {
        return false;
    }
    return true;
}

bool ring_buffer::del()
{
    if (head_index == tail_index)
    {
        return false;
    }
    ++head_index;

    if (head_index > RB_SIZE )
    {
        // lock here?
        head_index = (head_index + 1) % RB_SIZE;
    }
}
bool ring_buffer::is_empty()
{
    return (tail_index == head_index);
}

bool ring_buffer::init()
{
    for (int i = 0; i < RB_SIZE, i++ )
    {
        char* tmp_ele_buffer = new char[RB_ELEMENT_SIZE];
        if(tmp_ele_buffer)
        {
            _ring_buffer[i] = tmp_ele_buffer;
        }
        else
        {
            // log here
        }
    }
    return true;
}
bool ring_buffer::destroy()
{
    for (int i = 0; i < RB_SIZE, i++ )
    {
        char* tmp_ele_buffer = _ring_buffer[i];
        delete [] tmp_ele_buffer;
    }
    return true; 
}

int main()
{
        
}
