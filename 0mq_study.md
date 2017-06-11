## error handling
```
void *ctx = zmq_init (1, 1, 0);
if (!ctx) {
    printf ("Error occurred during zmq_init(): %s\n", zmq_strerror (errno));
abort (); 

}
```
