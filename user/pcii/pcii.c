#include <stdio.h>
#include <fcntl.h>
#include <unistd.h>
#include <dirent.h>
#include <string.h>

typedef struct PCIFSInfo
{
    unsigned short Bus;
    unsigned short Slot;
    unsigned short Function;

    unsigned char ClassCode;
    unsigned char SubClass;

    unsigned short Vendor;
    unsigned short Device;
} __attribute__((packed)) pciInfo;

void print_device(char *name)
{
    unsigned char pciinfo[sizeof(pciInfo)];
    char totalPath[30];

    sprintf(totalPath, "pci://%s/info", name);

    FILE *fp = fopen(totalPath, "r");

    fread(pciinfo, sizeof(char), sizeof(pciInfo), fp);

    pciInfo *info = (pciInfo *)pciinfo;

    printf("|\t%02d\t|\t%02d\t|\t%02d\t|\t%04x\t|\t%04x\t|\t%02x\t|\t\t%02x\t\t|\n", info->Bus, info->Slot, info->Function, info->Vendor, info->Device, info->ClassCode, info->SubClass);

    fclose(fp);
}

int main(int argc, char* argv[])
{
    DIR *dir;
    struct dirent *ent;
    if ((dir = opendir ("pci://")) != NULL) {
        printf("|-------|-------|-------|-----------|-----------|-------|---------------|\n");
        printf("|  BUS  |  SLOT |  FUNC |   VENDOR  |   DEVICE  | CLASS |    SUBCLASS   |\n");
        printf("|-------|-------|-------|-----------|-----------|-------|---------------|\n");
        /* print all the files and directories within directory */
        while ((ent = readdir (dir)) != NULL) {
            print_device(ent->d_name);
        }
        closedir (dir);
        printf("|-------|-------|-------|-----------|-----------|-------|---------------|\n");
    } else {

        return -1;
    }

    return 0;
}
