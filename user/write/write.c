#include <stdio.h>
#include <fcntl.h>
#include <unistd.h>
#include <stdlib.h>
#include <string.h>

int main(int argc, char* argv[])
{
    if(argc <= 1)
    {
        printf("Usage: write filename\n");

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

        // Exclude newline
        int len = strlen((char *)word) - 1;

        if(word[0] == '\n')
            break;
        
        if(written != 0)
            write(fd, "\n", 1);
        write(fd, (char *)word, len);

        written += len;
    }

    close(fd);

    printf("%d characters written\n", written);

    return 0;
}