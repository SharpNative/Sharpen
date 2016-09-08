#include <stdio.h>
#include <string.h>
#include <time.h>
#include <unistd.h>
#include <setjmp.h>

int main(int argc, char* argv[])
{
    // Time test
    time_t rawtime;
    time(&rawtime);
    struct tm* timeInfo = localtime(&rawtime);

    char buffer[256];
    memset(buffer, 0, sizeof(buffer));
    strftime(buffer, sizeof(buffer), "%c", timeInfo);
    buffer[sizeof(buffer) - 1] = '\0';
    printf("%s\n", buffer);

    // Argument test
    for(int i = 0; i < argc; i++)
        printf("-> %s\n", argv[i]);

    // Pipe test
    int fd[2];
    char* str = "This is a pipe test";
    char readbuffer[100];

    pipe(fd);

    pid_t pid = fork();
    if(pid == 0)
    {
        write(fd[1], str, strlen(str) + 1);
    }
    else
    {
        size_t a = read(fd[0], readbuffer, sizeof(readbuffer));
        printf("received: (%d) %s\n", (int)a, readbuffer);
    }

    return 0;
}