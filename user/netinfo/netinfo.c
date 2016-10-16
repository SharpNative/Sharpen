#include <stdio.h>
#include <fcntl.h>
#include <unistd.h>
#include <stdlib.h>

int shutdown(void);
int sched_yield(void);

/* This init process is the base of everything */
/* This is meant to initialize userspace and to let the user log in */
/* If the last process exits, the waitpid will stop waiting */
/* So we can shutdown */

int main(int argc, char* argv[])
{

    unsigned char *buffer = (unsigned char *)malloc(4); 

    int file = open("net://info/ip", O_RDONLY);
    read(file, buffer, 4);
    close(file);

    printf("IP: %d.%d.%d.%d\n", buffer[0], buffer[1], buffer[2], buffer[3]);
    
    file = open("net://info/gateway", O_RDONLY);
    read(file, buffer, 4);
    close(file);

    printf("Gateway: %d.%d.%d.%d\n", buffer[0], buffer[1], buffer[2], buffer[3]);
    

    file = open("net://info/subnet", O_RDONLY);
    read(file, buffer, 4);
    close(file);

    printf("Netmask: %d.%d.%d.%d\n", buffer[0], buffer[1], buffer[2], buffer[3]);
    

    file = open("net://info/ns1", O_RDONLY);
    read(file, buffer, 4);
    close(file);

    printf("DNS1: %d.%d.%d.%d\n", buffer[0], buffer[1], buffer[2], buffer[3]);
    
    file = open("net://info/ns2", O_RDONLY);
    read(file, buffer, 4);
    close(file);

    printf("DNS2: %d.%d.%d.%d\n", buffer[0], buffer[1], buffer[2], buffer[3]);

    free(buffer);

    return 0;
}