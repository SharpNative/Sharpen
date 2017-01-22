#include <stdio.h>
#include <fcntl.h>
#include <unistd.h>
#include <dirent.h>
#include <string.h>

typedef struct
{
    char Name[64];
    char CMDLine[256];
    pid_t Pid;
    unsigned int Uptime;
    int Priority;
    int ThreadCount;
} ProcFSInfo;

void print_process(char* name)
{
    ProcFSInfo info;
    char totalPath[30];

    sprintf(totalPath, "proc://%s/info", name);

    FILE *fp = fopen(totalPath, "r");

    fread(&info, 1, sizeof(ProcFSInfo), fp);

    int prio = info.Priority;
    char* prioName = "unknown";
    switch(prio)
    {
        case 1:
            prioName = "Very low";
            break;

        case 3:
            prioName = "Low";
            break;

        case 6:
            prioName = "Normal";
            break;

        case 9:
            prioName = "High";
            break;

        case 12:
            prioName = "Very high";
            break;
    }

    printf("%s (PID: %d)\n\tThreads: %d\n\tPriority: %s\n\tUptime: %d\n\tCMDLine: %s\n", info.Name, info.Pid, info.ThreadCount, prioName, info.Uptime, info.CMDLine);

    fclose(fp);
}

int main(int argc, char* argv[])
{
    DIR *dir;
    struct dirent *ent;
    if ((dir = opendir ("proc://")) != NULL)
    {
        while ((ent = readdir (dir)) != NULL)
        {
            print_process(ent->d_name);
        }
        closedir (dir);
    }
    else
    {
        return -1;
    }

    return 0;
}
