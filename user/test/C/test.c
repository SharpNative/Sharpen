#include <stdio.h>
#include <string.h>
#include <time.h>

int main(int argc, char* argv[])
{
    time_t rawtime;
    time(&rawtime);
    struct tm* timeInfo = localtime(&rawtime);

    char buffer[256];
    memset(buffer, 0, sizeof(buffer));
    strftime(buffer, sizeof(buffer), "%c", timeInfo);
    buffer[sizeof(buffer) - 1] = '\0';
    printf("%s\n", buffer);

    return 0;
}