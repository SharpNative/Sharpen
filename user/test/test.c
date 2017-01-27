#include <stdio.h>
#include <signal.h>
#include <unistd.h>

void handler(int num)
{
    printf("Received signal %d\n", num);
    register int esp __asm__("esp");
    printf("ESP=%x\n", esp);
}

int main(int argc, char* argv[])
{
    if(signal(SIGINT, handler) == SIG_ERR)
        puts("Cannot catch SIGINT");
    if(signal(SIGKILL, handler) == SIG_ERR)
        puts("Cannot catch SIGKILL");
    if(signal(SIGSTOP, handler) == SIG_ERR)
        puts("Cannot catch SIGSTOP");

    register int esp __asm__("esp");
    printf("ESP=%x\n", esp);
    
    sleep(3);
    volatile float a = 3.0f;
    raise(SIGINT);
    puts("Returned from signal");
    volatile float c = 333.0f;
    volatile float d = c / a;
    printf("%g\n", d);
    sleep(3);

    return 0;
}