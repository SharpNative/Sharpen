#include <stdio.h>
#include <fcntl.h>
#include <unistd.h>
#include <sys/wait.h>

int shutdown(void);
int reboot(void);
pid_t run(const char* path, const char** argv, const char** envp);

/* This init process is the base of everything */
/* This is meant to initialize userspace and to let the user log in */
/* If the last process exits, the waitpid will stop waiting */
/* So we can shutdown */

int main(int argc, char* argv[])
{
    // Initialize standard IO devices
    (void) open("devices://keyboard", O_RDONLY); // STDIN
    (void) open("devices://stdout",   O_WRONLY); // STDOUT
    (void) open("devices://stdout",   O_WRONLY); // STDERR
    
    // Check
    if(argc != 2)
    {
        puts("Wrong usage of init: init [program]");
        return 1;
    }

    // TODO: launch login, set userid, groupid, ...
    // But we're not there yet
    // ...

    printf("init: Launching %s\n", argv[1]);
    
    // Launch program
    const char* args[] = { argv[1], NULL };
    pid_t pid = run(args[0], args, NULL);

    // Wait for program to exit
    waitpid(pid, NULL, 0);

    puts("init: Child stopped, shutting down...");

    // Done, shutdown
    if(shutdown())
    {
        puts("Shutdown failed...");
    }

    return 0;
}