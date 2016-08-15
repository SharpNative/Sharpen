#include <stdio.h>
#include <fcntl.h>
#include <unistd.h>

int main(int argc, char* argv[])
{
    // Initializing STDIO is actually part of the inital userspace process
    // and not a regular program...
    // Forking results in copying file descriptors, so initial process should fork
    (void) open("devices://keyboard", O_RDONLY);
    (void) open("devices://stdout",   O_WRONLY);
    (void) open("devices://stdout",   O_WRONLY);

    printf("Hello world!\n");

    char data[64];
    fgets(data, sizeof data, stdin);
    puts(data);
    fflush(stdout);

    register int sp __asm__ ("sp");
    printf("SP=%x\n", sp);

    puts("Forking...");
    pid_t pid = fork();

register int sp2 __asm__ ("sp");
    printf("SP=%x\n", sp2);

    printf("PID=%d\n", pid);

    fflush(stdout);
    return 0;
}