#include <stdio.h>
#include <fcntl.h>
#include <unistd.h>
#include <stdlib.h>

int main(int argc, char* argv[])
{
    int file = open("net://arp/192.168.10.10", O_RDONLY);
    if(file == -1)
    {
        printf("ARP request done\n");
    }
    else
    {
        printf("Already in arp\n");
        close(file);
    }

    return 0;
}