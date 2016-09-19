#include <stdio.h>
#include <fcntl.h>
#include <unistd.h>
#include <sys/wait.h>
#include <string.h>
#include <sched.h>
#include <time.h>

int shutdown(void);
int sched_yield(void);

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

    printf("init: Launching %s\n", argv[1]);

    // TODO: should be moved to a terminal instead of here in init
    int in[2];
    // 0 is read end, 1 is write end
    pipe(in);

    // Child process
    pid_t pid = fork();
    if(pid == 0)
    {
        // Child, replace STDIO descriptors
        // Close write end
        close(STDIN_FILENO);
        close(in[1]);
        dup2(STDIN_FILENO, in[0]);

        // Launch
        char* args[] = { argv[1], NULL };
        execve(args[0], args, NULL);
    }
    else
    {
        // Parent, handle IO
        // Close read end
        close(in[0]);
        
        volatile pid_t child = pid;

        // Keep handling
        static char input[1024];
        int index = 0;
        while(1)
        {
            struct stat st;

            // Handle stdin
            fstat(STDIN_FILENO, &st);
            if(st.st_size > 0)
            {
                // Data in stdin, read and process it
                char buffer[st.st_size];
                read(STDIN_FILENO, buffer, st.st_size);

                for(int i = 0; i < st.st_size; i++)
                {
                    char ch = buffer[i];

                    // Backspace
                    if(ch == '\b')
                    {
                        if(index > 0)
                        {
                            input[--index] = '\0';
                            char out[] = { '\b', ' ', '\b' };
                            write(STDOUT_FILENO, out, sizeof(out) / sizeof(char));
                        }
                    }
                    // Send buffer
                    else if(ch == '\n')
                    {
                        input[index++] = '\n';
                        write(in[1], input, index);
                        index = 0;
                        char out[] = { ch };
                        write(STDOUT_FILENO, out, sizeof(out) / sizeof(char));
                    }
                    // All other characters
                    else
                    {
                        input[index++] = ch;
                        char out[] = { ch };
                        write(STDOUT_FILENO, out, sizeof(out) / sizeof(char));
                    }

                    // Buffer full? Send it
                    if(index == sizeof(input))
                    {
                        write(in[1], input, index);
                        index = 0;
                    }
                }
            }

            // No input, yield
            sched_yield();

            // Wait for program to exit
            if(waitpid(child, NULL, WNOHANG) != 0)
                break;
        }
    }
    
    puts("init: Child stopped, shutting down...");
    // TODO: sleep 3 seconds

    // Done, shutdown
    if(shutdown())
    {
        puts("Shutdown failed...");
    }

    return 0;
}