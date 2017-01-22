#include <stdio.h>
#include <fcntl.h>
#include <unistd.h>
#include <dirent.h>
#include <string.h>

typedef struct
{
    unsigned short Bus;
    unsigned short Slot;
    unsigned short Function;

    unsigned char ClassCode;
    unsigned char SubClass;
    unsigned char ProgIntf;

    unsigned short Vendor;
    unsigned short Device;
} PCIFSInfo;

void print_device(char* name)
{
    PCIFSInfo info;
    char totalPath[30];

    sprintf(totalPath, "pci://%s/info", name);

    FILE *fp = fopen(totalPath, "r");

    fread(&info, 1, sizeof(PCIFSInfo), fp);

    printf("|\t%02d\t|\t%02d\t|\t%02d\t|\t%04x\t|\t%04x\t|\t%02x\t|\t%02x\t|\t%02x\t|\n", info.Bus, info.Slot, info.Function, info.Vendor, info.Device, info.ClassCode, info.SubClass, info.ProgIntf);

    fclose(fp);
}

int main(int argc, char* argv[])
{
    DIR *dir;
    struct dirent *ent;
    if ((dir = opendir ("pci://")) != NULL) {
        printf("|-------|-------|-------|-----------|-----------|-------|-------|-------|\n");
        printf("|  BUS  |  SLOT |  FUNC |   VENDOR  |   DEVICE  | CLASS |  SUB  | INTF  |\n");
        printf("|-------|-------|-------|-----------|-----------|-------|-------|-------|\n");
        /* print all the files and directories within directory */
        while ((ent = readdir (dir)) != NULL) {
            print_device(ent->d_name);
        }
        closedir (dir);
        printf("|-------|-------|-------|-----------|-----------|-------|-------|-------|\n");
    } else {

        return -1;
    }

    return 0;
}
