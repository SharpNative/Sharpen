#include <sys/stat.h>
#include <sys/types.h>
#include <sys/fcntl.h>
#include <sys/times.h>
#include <sys/time.h>
#include <sys/errno.h>
#include <sys/time.h>
#include <stdio.h>
#include <stdint.h>
#include <dirent.h>
#include <string.h>
#include <malloc.h>
#include <setjmp.h>

/* Cannot use printf here because it depends on other functions etc */
#define UNIMPLEMENTED   {\
                            sys_write(1, "[USER/SYSCALL] Unimplemented ", sizeof("[USER/SYSCALL] Unimplemented "));\
                            sys_write(1, (char*)__FUNCTION__, sizeof(__FUNCTION__));\
                            sys_write(1, "\n", 1);\
                        }

#undef errno
extern int errno;

// =================================== //
// ---   Syscall helpers macros    --- //
// =================================== //
#define SYS0(name, num) \
inline static int sys_##name() \
{ \
    int ret; \
    asm volatile("int $0x80" : "=a" (ret) : "a" (num)); \
    return ret; \
}

#define SYS1(name, num, A) \
inline static int sys_##name(A a) \
{ \
    int ret; \
    asm volatile("int $0x80" : "=a" (ret) : "a" (num), "b" ((int) a)); \
    return ret; \
}

#define SYS2(name, num, A, B) \
inline static int sys_##name(A a, B b) \
{ \
    int ret; \
    asm volatile("int $0x80" : "=a" (ret) : "a" (num), "b" ((int) a), "c" ((int) b)); \
    return ret; \
}

#define SYS3(name, num, A, B, C) \
inline static int sys_##name(A a, B b, C c) \
{ \
    int ret; \
    asm volatile("int $0x80" : "=a" (ret) : "a" (num), "b" ((int) a), "c" ((int) b), "d" ((int) c)); \
    return ret; \
}

#define SYS4(name, num, A, B, C, D) \
inline static int sys_##name(A a, B b, C c, D d) \
{ \
    int ret; \
    asm volatile("int $0x80" : "=a" (ret) : "a" (num), "b" ((int) a), "c" ((int) b), "d" ((int) c), "S", ((int) d)); \
    return ret; \
}

#define SYS5(name, num, A, B, C, D, E) \
inline static int sys_##name(A a, B b, C c, D d, E e) \
{ \
    int ret; \
    asm volatile("int $0x80" : "=a" (ret) : "a" (num), "b" ((int) a), "c" ((int) b), "d" ((int) c), "S", ((int) d), "D" ((int) e)); \
    return ret; \
}

typedef void (*sighandler_t)(int);

// =================================== //
// ---     Syscall definitions     --- //
// =================================== //
SYS1(exit,           0, int);
SYS0(getpid,         1);
SYS1(sbrk,           2, int);
SYS0(fork,           3);
SYS3(write,          4, int, void*, int);
SYS3(read,           5, int, void*, int);
SYS2(open,           6, const char*, int);
SYS1(close,          7, int);
SYS3(seek,           8, int, int, int);
SYS2(fstat,          9, int, struct stat*);
SYS2(stat,          10, const char*, struct stat*);
SYS3(execve,        11, const char*, const char**, const char**);
SYS3(run,           12, const char*, const char**, const char**);
SYS3(waitpid,       13, int, int*, int);
SYS3(readdir,       14, int, struct dirent*, uint32_t);
SYS0(shutdown,      15);
SYS0(reboot,        16);
SYS1(gettimeofday,  17, struct timeval*);
SYS1(pipe,          18, int*);
SYS2(dup2,          19, int, int);
SYS2(sig_send,      20, int, int);
SYS2(sig_handler,   21, int, sighandler_t);
SYS0(yield,         22);


// =================================== //
// --- Implementations of methods  --- //
// =================================== //

char *__env[1] = { 0 };
char **environ = __env;

pid_t getpid(void)
{
    return sys_getpid();
}

void _exit(int status)
{
    sys_exit(status);
}

int gettimeofday(struct timeval* tv, void* tz)
{
    // The use of the timezone structure (tz) is obsolete
    int ret = sys_gettimeofday(tv);
    if(ret < 0)
    {
        errno = -ret;
        return -1;
    }

    return 0;
}

int lseek(int file, off_t offset, int whence)
{
    int ret = sys_seek(file, (int)offset, whence);
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

int read(int file, void* ptr, size_t len)
{
    int ret = sys_read(file, ptr, (int)len);
    if(ret < 0)
    {
        errno = -ret;
        return -1;
    }

    return ret;
}

int write(int file, void* ptr, size_t len)
{
    int ret = sys_write(file, ptr, (int)len);
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
    int ret = sys_stat(file, st);
    if(ret < 0)
    {
        memset(st, 0, sizeof(struct stat));
        errno = -ret;
        return -1;
    }

    return 0;
}

int fstat(int file, struct stat* st)
{
    int ret = sys_fstat(file, st);
    if(ret < 0)
    {
        memset(st, 0, sizeof(struct stat));
        errno = -ret;
        return -1;
    }

    return 0;
}

int unlink(const char* name)
{
    UNIMPLEMENTED;
    errno = ENOENT;
    return -1;
}

clock_t times(struct tms* buf)
{
    UNIMPLEMENTED;
    return (clock_t)-1;
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

int run(const char* path, const char** argv, const char** envp)
{
    int ret = sys_run(path, argv, envp);
    if(ret < 0)
    {
        errno = -ret;
        return -1;
    }

    return ret;
}

int waitpid(int pid, int* status, int options)
{
    int ret = sys_waitpid(pid, status, options);
    if(ret < 0)
    {
        errno = -ret;
        return -1;
    }

    return ret;
}

int wait(int* status)
{
    return waitpid(-1, status, 0);
}

int execv(const char* path, const char** argv)
{
    return execve(path, argv, (const char**) environ);
}

caddr_t sbrk(int incr)
{
    return (caddr_t) sys_sbrk(incr);
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

int shutdown(void)
{
    int ret = sys_shutdown();
    if(ret < 0)
    {
        errno = -ret;
        return -1;
    }

    return 0;
}

int reboot(void)
{
    int ret = sys_reboot();
    if(ret < 0)
    {
        errno = -ret;
        return -1;
    }

    return 0;
}

int isatty(int file)
{
    return (file < 3);
}

int kill(int pid, int sig)
{
    int ret = sys_sig_send(pid, sig);
    if(ret < 0)
    {
        errno = -ret;
        return -1;
    }

    return 0;
}

sighandler_t signal(int sig, sighandler_t handler)
{
    return (sighandler_t)sys_sig_handler(sig, handler);
}

int link(char* old, char* new)
{
    UNIMPLEMENTED;
    errno = EMLINK;
    return -1;
}

DIR* opendir(const char *name)
{
    /* Open the file, if it doesn't exist: stop! */
    int descriptor = open(name, O_RDONLY);
    if(descriptor < 0)
    {
        errno = ENOENT;
        return NULL;
    }

    /* Create directory structure */
    DIR *dir = (DIR *)calloc(1, sizeof(DIR));
    dir->descriptor = descriptor;

    return dir;
}

struct dirent* readdir(DIR *dir)
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

FILE* popen(const char* command, const char* mode)
{
    UNIMPLEMENTED;
    errno = EMFILE;
    return NULL;
}

int pclose(FILE* stream)
{
    UNIMPLEMENTED;
    errno = ECHILD;
    return -1;
}

void flockfile(FILE* filehandle)
{
    UNIMPLEMENTED;
    return;
}

void funlockfile(FILE* filehandle)
{
    UNIMPLEMENTED;
    return;
}

int pipe(int pipefd[])
{
    int ret = sys_pipe(pipefd);
    if(ret < 0)
    {
        errno = -ret;
        return -1;
    }

    return 0;
}

int dup2(int oldfd, int newfd)
{
    int ret = sys_dup2(oldfd, newfd);
    if(ret < 0)
    {
        errno = -ret;
        return -1;
    }

    return ret;
}

int sched_yield(void)
{
    return sys_yield();
}