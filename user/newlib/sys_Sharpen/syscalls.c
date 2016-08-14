#include <sys/stat.h>
#include <sys/types.h>
#include <sys/fcntl.h>
#include <sys/times.h>
#include <sys/errno.h>
#include <sys/time.h>
#include <stdio.h>

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

// =================================== //
// --- Implementations of methods  --- //
// =================================== //

char *__env[1] = { 0 };
char **environ = __env;

void _exit()
{
    sys_exit(0);
}

int lseek(int file, int offset, int dir)
{
    int ret = sys_seek(file, offset, dir);
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

int execve(char* name, char** argv, char** env)
{
    errno = ENOMEM;
    return -1;
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
    errno = EAGAIN;
    return -1;
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