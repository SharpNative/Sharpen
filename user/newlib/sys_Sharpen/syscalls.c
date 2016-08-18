#include <sys/stat.h>
#include <sys/types.h>
#include <sys/fcntl.h>
#include <sys/times.h>
#include <sys/errno.h>
#include <sys/time.h>
#include <stdio.h>
#include <stdint.h>
#include <dirent.h>
#include <string.h>
#include <malloc.h>

#undef errno
extern int errno;

// =================================== //
// ---   Syscall helpers macros    --- //
// =================================== //
#define SYS0(name, num) \
int sys_##name() \
{ \
    int ret; \
    asm volatile("int $0x80" : "=a" (ret) : "a" (num)); \
    return ret; \
}

#define SYS1(name, num, A) \
int sys_##name(A a) \
{ \
    int ret; \
    asm volatile("int $0x80" : "=a" (ret) : "a" (num), "b" ((int) a)); \
    return ret; \
}

#define SYS2(name, num, A, B) \
int sys_##name(A a, B b) \
{ \
    int ret; \
    asm volatile("int $0x80" : "=a" (ret) : "a" (num), "b" ((int) a), "c" ((int) b)); \
    return ret; \
}

#define SYS3(name, num, A, B, C) \
int sys_##name(A a, B b, C c) \
{ \
    int ret; \
    asm volatile("int $0x80" : "=a" (ret) : "a" (num), "b" ((int) a), "c" ((int) b), "d" ((int) c)); \
    return ret; \
}

#define SYS4(name, num, A, B, C, D) \
int sys_##name(A a, B b, C c, D d) \
{ \
    int ret; \
    asm volatile("int $0x80" : "=a" (ret) : "a" (num), "b" ((int) a), "c" ((int) b), "d" ((int) c), "S", ((int) d)); \
    return ret; \
}

#define SYS5(name, num, A, B, C, D, E) \
int sys_##name(A a, B b, C c, D d, E e) \
{ \
    int ret; \
    asm volatile("int $0x80" : "=a" (ret) : "a" (num), "b" ((int) a), "c" ((int) b), "d" ((int) c), "S", ((int) d), "D" ((int) e)); \
    return ret; \
}

// =================================== //
// ---     Syscall definitions     --- //
// =================================== //
SYS1(exit,     0, int);
SYS0(getpid,   1);
SYS1(sbrk,     2, int);
SYS0(fork,     3);
SYS3(write,    4, int, char*, int);
SYS3(read,     5, int, char*, int);
SYS2(open,     6, const char*, int);
SYS1(close,    7, int);
SYS3(seek,     8, int, int, int);
SYS3(execve,   9, const char*, const char**, const char**);
SYS3(readdir, 10, int, struct dirent*, uint32_t);

// =================================== //
// --- Implementations of methods  --- //
// =================================== //

char *__env[1] = { 0 };
char **environ = __env;

void _exit()
{
    sys_exit(0);
}

int lseek(int file, int ptr, int dir)
{
    int ret = sys_seek(file, ptr, dir);
    if(ret < 0)
    {
        errno = -ret;
        return -1;
    }

    return ret;
}

int open(const char* name, int flags, ...)
{
    int ret = sys_open(name, flags);
    if(ret < 0)
    {
        errno = -ret;
        return -1;
    }

    return ret;
}

int read(int file, char* ptr, int len)
{
    int ret = sys_read(file, ptr, len);
    if(ret < 0)
    {
        errno = -ret;
        return -1;
    }

    return ret;
}

int write(int file, char* ptr, int len)
{
    int ret = sys_write(file, ptr, len);
    if(ret < 0)
    {
        errno = -ret;
        return -1;
    }

    return ret;
}

int close(int file)
{
    int ret = sys_close(file);
    if(ret < 0)
    {
        errno = -ret;
        return -1;
    }

    return ret;
}

int stat(const char* file, struct stat* st)
{
    st->st_mode = S_IFCHR;
    return 0;
}

int unlink(char* name)
{
    errno = ENOENT;
    return -1;
}

clock_t times(struct tms* buf)
{
    return (clock_t)-1;
}

int wait(int* status)
{
    errno = ECHILD;
    return -1;
}

int execve(const char* path, const char** argv, const char** envp)
{
    int ret = sys_execve(path, argv, envp);
    if(ret < 0)
    {
        errno = -ret;
        return -1;
    }

    return 0;
}

int execv(const char* path, const char** argv)
{
    return execve(path, argv, (const char**) environ);
}

caddr_t sbrk(int incr)
{
    return (caddr_t) sys_sbrk(incr);
}

int getpid(void)
{
    return sys_getpid();
}

int fork(void)
{
    int ret = sys_fork();
    if(ret < 0)
    {
        errno = -ret;
        return -1;
    }

    return ret;
}

int fstat(int file, struct stat* st)
{
    st->st_mode = S_IFCHR;
    return 0;
}

int isatty(int file)
{
    return (file < 3);
}

int kill(int pid, int sig)
{
    errno = EINVAL;
    return -1;
}

int link(char* old, char* new)
{
    errno = EMLINK;
    return -1;
}

DIR *opendir(const char *name)
{
    /* Open the file, if it doesn't exist: stop! */
    int descriptor = open(name, O_RDONLY);
    if(descriptor < 0)
    {
        errno = ENOENT;
        return NULL;
    }

    /* Create directory structure */
    DIR *dir = (DIR *) calloc(1, sizeof(DIR));
    dir->descriptor = descriptor;

    return dir;
}

struct dirent *readdir(DIR *dir)
{
    if(dir == NULL)
    {
        errno = EINVAL;
        return NULL;
    }

    int ret = sys_readdir(dir->descriptor, &dir->__current, dir->last);
    if(ret < 0)
    {
        memset(&dir->__current, 0, sizeof(struct dirent));
        errno = -ret;
        return NULL;
    }

    dir->last++;
    return &dir->__current;
}

int closedir(DIR *dir)
{
    if(dir == NULL)
    {
        errno = EINVAL;
        return -1;
    }

    int descriptor = dir->descriptor;
    free(dir);
    return close(descriptor);
}