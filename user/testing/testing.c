#include <stdio.h>
#include <fcntl.h>
#include <unistd.h>
#include <stdlib.h>

int main(int argc, char* argv[])
{
    int file = open("net://udp/connect/192.168.10.13:11000", O_RDWR);
    if(file < 0)
    {
        printf("Cant find!\n");
        return 0;
    }

    unsigned char *buffer = (unsigned char *)malloc(4); 

    buffer[0] = 'A';
    buffer[1] = '\0';

    write(file, buffer, 2);

    int i = 1;
    while(read(file, buffer, 4) == 0 && i > 0)
        i++;

    buffer[3] = '\0';

    printf("JA: %s\n", buffer);

    close(file);


    return 0;
}