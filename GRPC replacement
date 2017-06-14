# glob rule: maxmum reuse the current arc
##common
### service definition
```
enum ServiceType {
    DB_OPERATION = 0,
    DB_GET_STATUS = 1,
    DB_INIT = 2,
    DB_EXPRESSION = 3,
    MAX_TYPE
};
```
### message ID

## client side
A message form client side should contain the following part:
1. message ID
2. service type
3. message body


## server side
server side should receive message with the following part:
1. Identity(use in the ROUTER to route the message to the right client)
2. message ID(used to find the callback function and user data saved in the client side)
3. service type(used to choose message parser)
4. message body(message body, reuse the currently google protocol buffer API)

### in the server side we should remember a map uniqueID<->message part 1,2,3(we can resue ZMSG, pop the message data to save memory?)
