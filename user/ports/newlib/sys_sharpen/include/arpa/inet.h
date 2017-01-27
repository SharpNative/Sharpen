#ifndef __ARPA_INET
#define __ARPA_INET

#include <inttypes.h>

uint32_t htonl(uint32_t hostlong);
uint32_t ntohl(uint32_t netlong);
uint16_t htons(uint16_t hostshort);
uint16_t ntohs(uint16_t netshort);

#endif
