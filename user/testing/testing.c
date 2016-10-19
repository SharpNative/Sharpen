#include <stdio.h>
#include <fcntl.h>
#include <unistd.h>
#include <stdlib.h>

typedef struct  
{
    unsigned char IP[4];
    int size;
} UDPMessageHeader;

int main(int argc, char* argv[])
{
    int file = open("net://udp/bind/999", O_RDWR);
    printf("%d\n", file);
    if(file < 0)
    {
        printf("Bind port failed!\n");
        return 0;
    }

    unsigned char *buffer = (unsigned char *)malloc(10);

    int i = 1;
    while(read(file, buffer, 10) == 0 && i > 0)
        i++;

    UDPMessageHeader *header = (UDPMessageHeader *)buffer;

    printf("\n\nTest: %d.%d.%d.%d\n\n", header->IP[0], header->IP[1], header->IP[2], header->IP[3]);

    close(file);


    return 0;
}