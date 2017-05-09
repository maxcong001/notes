#include <sys/socket.h>
#include <arpa/inet.h>

bool getDestIPAndPort(redisAsyncContext *context, HostAndPort &destInfo)
{
    if(!context)
    {
        logger->error(DBW_LOG, "Invalid pointer.\n");
        return;
    }
    int sockId = context->c.fd;

    struct sockaddr_storage destAddr;
    void *numericAddress;
    in_port_t port = 0;
    char addrBuffer[INET6_ADDRSTRLEN];

    socklen_t destAddrLen = sizeof(destAddr);
    if (getpeername(sockId, (struct sockaddr*)&destAddr, &destAddrLen) == 0){
        switch(((struct sockaddr*)&destAddr)->sa_family)
        {
            case AF_INET:
                numericAddress = &((struct sockaddr_in *)&destAddr)->sin_addr;
                port = ntohs(((struct sockaddr_in *)&destAddr)->sin_port);
                break;
            case AF_INET6:
                numericAddress = &((struct sockaddr_in6 *)&destAddr)->sin6_addr;
                port = ntohs(((struct sockaddr_in6 *)&destAddr)->sin6_port);
                break;
            default:
                logger->error(DBW_LOG, "Unknow sockect type.\n");
                return;
        }
    }else{
        logger->error(DBW_LOG, "Fail to get dest IP from socket for context %p\n.", context);
        return;
    }

    if(inet_ntop(((struct sockaddr*)&destAddr)->sa_family, numericAddress, addrBuffer, sizeof(addrBuffer)) == NULL){
        logger->error(DBW_LOG, "Invalid address for context %p.\n", context);
        return;
    }

    destInfo.host = addrBuffer;
    destInfo.port = port;
    logger->debug(DBW_LOG, "Src info is (%s:%d) for context %p.\n", destInfo.host.c_str(), destInfo.port, context);

    return;
}

BaseConnManager::ptr_t mgrPtr = asyncClient->getPool();


DwSvcDiscoveryInfo* svcInfo = reinterpret_cast<DwSvcDiscoveryInfo*>(mgrPtr->getSvcDiscInfo());

DwHostAndPortSet intSetPerDest;
tempAddr.port = destInfo.host;
tempAddr.host = destInfo.port;

if (svcInfo->getMappedAddr(TIS, DwAddrType::DESTINATION_ADDR, DwAddrType::EXTERNAL_ADDR,  tempAddr, intSetPerDest) == ROK)
{

}
else
{

}

//till now we have destIP(internal) and dsetIP(external) and context


if (mode == ConnectionMode::DBW_DISPATCHER_MODE)
{

}
else if (mode == ConnectionMode::DBW_DIRECT_MODE)
{

}
else
{
    logger->error(DBW_LOG,"unsupport mode : %d.\n", mode);
}
