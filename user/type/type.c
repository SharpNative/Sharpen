#include <stdio.h>
#include <fcntl.h>
#include <unistd.h>
#include <stdlib.h>
#include <string.h>

#define MAXBUFLEN 4096

int main(int argc, char* argv[])
{
    if(argc <= 1)
    {
        printf("Usage: type filename <<max length>>\n");

        return 0;
    }

    char *filename = argv[1];
    long bufsize = -1;

    if(argc > 2)
    {
        bufsize = strtol(argv[2], NULL, 10);
    }

    FILE *fp = fopen(filename, "r");

    if(fp == NULL)
    {
        printf("File does not exists\n");
        return 0;
    }

    char *source = NULL;
    if (fseek(fp, 0L, SEEK_END) == 0)
    {
        if(bufsize == -1)
        {
            bufsize = ftell(fp);
            if (bufsize == -1) { 
                printf("Cannot read file\n");
                fclose(fp);

                return 0;
            }
        }

        if (fseek(fp, 0L, SEEK_SET) != 0) 
        {
            printf("Cannot read file\n");
            fclose(fp);
            return 0;
        }

        source = malloc(sizeof(char) * (bufsize + 1));

        size_t newLen = fread(source, sizeof(char), bufsize, fp);
        if (newLen == 0) {
            printf("Cannot read file\n");
        } else {
            source[++newLen] = '\0';
        }
    }

    printf("%s\n", source);
    free(source);
    fclose(fp);
    return 0;
}