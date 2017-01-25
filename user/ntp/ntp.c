#include <arpa/inet.h>
#include <stdio.h>
#include <fcntl.h>
#include <unistd.h>
#include <stdlib.h>
#include <string.h>

typedef struct
{
    unsigned li   : 2;       // Only two bits. Leap indicator.
    unsigned vn   : 3;       // Only three bits. Version number of the protocol.
    unsigned mode : 3;       // Only three bits. Mode. Client will pick mode 3 for client.

    uint8_t stratum;         // Eight bits. Stratum level of the local clock.
    uint8_t poll;            // Eight bits. Maximum interval between successive messages.
    uint8_t precision;       // Eight bits. Precision of the local clock.

    uint32_t rootDelay;      // 32 bits. Total round trip delay time.
    uint32_t rootDispersion; // 32 bits. Max error aloud from primary clock source.
    uint32_t refId;          // 32 bits. Reference clock identifier.

    uint32_t refTm_s;        // 32 bits. Reference time-stamp seconds.
    uint32_t refTm_f;        // 32 bits. Reference time-stamp fraction of a second.

    uint32_t origTm_s;       // 32 bits. Originate time-stamp seconds.
    uint32_t origTm_f;       // 32 bits. Originate time-stamp fraction of a second.

    uint32_t rxTm_s;         // 32 bits. Received time-stamp seconds.
    uint32_t rxTm_f;         // 32 bits. Received time-stamp fraction of a second.

    uint32_t txTm_s;         // 32 bits and the most important field the client cares about. Transmit time-stamp seconds.
    uint32_t txTm_f;         // 32 bits. Transmit time-stamp fraction of a second.
} ntp_packet;                // Total: 384 bits or 48 bytes.


#define NTP_TIMESTAMP_DELTA 2208988800ull

int main(int argc, char* argv[])
{
    int fd = open("net://udp/connect/93.94.224.67:123", O_RDWR);

    if(fd == -1)
    {
        printf("Could not open the connection.\n");
        return 0;
    }

    char* byte = (char*)malloc(48);
    memset(byte, 0, 48);
    byte[0] = 0x1B;

    write(fd, byte, 48);

    ntp_packet* packet = (ntp_packet*)malloc(sizeof(ntp_packet));

    read(fd, packet, 48);
    close(fd);

    packet->txTm_s = ntohl(packet->txTm_s);
    packet->txTm_f = ntohl(packet->txTm_f); 

    time_t txTm = (time_t)(packet->txTm_s - NTP_TIMESTAMP_DELTA);
    
    printf("Time: %s", ctime((const time_t*)&txTm));

    free(packet);
    free(byte);

    return 0;
}
