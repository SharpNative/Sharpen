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

    printf("Hello world!\n");
    printf("I got %d arguments, argv[0]=%p=%s\n", argc, argv[0], argv[0]);

    DIR* dir = opendir("devices://");
    if(dir == NULL)
    {
        puts("Cannot read directory");
        return 1;
    }

    struct dirent* entry;
    while((entry = readdir(dir)) != NULL)
    {
        printf("%s\n", entry->d_name);
    }

    closedir(dir);

    puts("---\ntesting execve");

    char* args[] = { "yow", NULL };
    execve("C://shell", args, NULL);

    return 0;
}