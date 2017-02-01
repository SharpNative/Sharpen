/* <dirent.h> includes <sys/dirent.h>, which is this file.  On a
   system which supports <dirent.h>, this file is overridden by
   dirent.h in the libc/sys/.../sys directory.  On a system which does
   not support <dirent.h>, we will get this file which uses #error to force
   an error.  */

#ifndef __DIRENT_H

#include <stdint.h>
#include <sys/types.h>

/* Structure of an entry in a directory */
struct dirent
{
    ino_t    d_ino;         /* Inode number of entry */
    off_t    d_offset;      /* Seek offset */
    uint8_t  d_type;        /* Type */
    uint16_t d_reclen;      /* Length of this record */
    char     d_name[256];   /* Filename, maximum length is 256 characters */
};

/* Structure of DIR */
typedef struct
{
    int           descriptor;   /* File descriptor of this directory */
    uint32_t      last;         /* Last read entry of this directory */
    struct dirent __current;    /* Copy of current directory entry */
} DIR;

DIR* opendir(const char *name);
int closedir(DIR* dir);
struct dirent* readdir(DIR* dir);

#endif