#include <stdio.h>
#include <fcntl.h>
#include <unistd.h>
#include <stdlib.h>

typedef struct  
{
    unsigned char IP[4];  // 4
    int size; // 8
} UDPMessageHeader;

int main(int argc, char* argv[])
{
    int file = open("net://udp/bind/999", O_RDWR);
    if(file < 0)
    {
        printf("Bind port failed!\n");
        return 0;
    }

    unsigned char *buffer = (unsigned char *)malloc(12);

    while(1)
    {
        int i = 1;
        while(read(file, buffer, 12) == 0 && i > 0)
            i++;

        UDPMessageHeader *header = (UDPMessageHeader *)buffer;
        unsigned char *ptr = buffer + sizeof(UDPMessageHeader);

        printf("\n\nTest: %d.%d.%d.%d Data: %x%x%x%x\n\n", header->IP[0], header->IP[1], header->IP[2], header->IP[3], ptr[0], ptr[1],  ptr[2],  ptr[3]);
    }
    
    close(file);


    return 0;
}