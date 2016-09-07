#include <stdio.h>
#include <fcntl.h>
#include <dirent.h>
#include <unistd.h>

int main(int argc, char* argv[])
{
    // Initializing STDIO is actually part of the inital userspace process
    // and not a regular program...
    // Forking results in copying file descriptors, so initial process should fork
    (void) open("devices://keyboard", O_RDONLY);
    (void) open("devices://stdout",   O_WRONLY);
    (void) open("devices://stdout",   O_WRONLY);

    puts("ik ben hier");

    char* args[] = { NULL };
    execve("C://lua", args, NULL);

    return 0;
}