#include <stdio.h>
#include <fcntl.h>
#include <unistd.h>
#include <stdlib.h>
#include <string.h>

int main(int argc, char* argv[])
{
    if(argc <= 1)
    {
        printf("Usage: Type filename\n");

        return 0;
    }

    char *filename = argv[1];

    int fd = open(filename, O_WRONLY);

    if(fd == -1)
    {
        printf("File not exists\n");

        return 0;
    }

    int written = 0;

    // Read until just an enter
    while(1)
    {
        char word[251];
        fgets (word, 250, stdin);

        int len = strlen((char *)word);

        if(word[0] == '\n')
            break;
        
        write(fd, (char *)word, len);

        written += len;
    }

    close(fd);

    printf("%d characters written\n", written);

    return 0;
}