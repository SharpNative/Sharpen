#include <machine/endian.h>
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
#include <stdarg.h>
#include <signal.h>

typedef void (*sighandler_t)(int, siginfo_t*, void*);

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
SYS3(seek,           8, int, off_t, int);
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
SYS3(sig_handler,   21, int, const struct sigaction*, struct sigaction*);
SYS2(getcwd,        23, char*, size_t);
SYS1(chdir,         24, const char*);
SYS1(times,         25, struct tms*);
SYS2(sleep,         26, uint32_t, uint32_t);
SYS2(truncate,      27, const char*, off_t);
SYS2(ftruncate,     28, int, off_t);
SYS1(dup,           29, int);
SYS3(ioctl,         30, int, int, void*);
SYS2(mkdir,         31, const char*, mode_t);
SYS1(rmdir,         32, const char*);
SYS1(unlink,        33, const char*);

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
    int ret = sys_seek(file, offset, whence);
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

int ioctl(int fd, int request, ...)
{
    va_list ap;
    va_start(ap, request);
    void* arg = va_arg(ap, void*);
    va_end(ap);

    int ret = sys_ioctl(fd, request, arg);
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
    int ret = sys_unlink(name);
    if(ret < 0)
    {
        errno = -ret;
        return -1;
    }

    return 0;
}

int rmdir(const char* name)
{
    int ret = sys_rmdir(name);
    if(ret < 0)
    {
        errno = -ret;
        return -1;
    }

    return 0;
}

int mkdir(const char* name, mode_t mode)
{
    int ret = sys_mkdir(name, mode);
    if(ret < 0)
    {
        errno = -ret;
        return -1;
    }

    return 0;
}

clock_t times(struct tms* buf)
{
    int ret = sys_times(buf);
    if(ret < 0)
    {
        errno = -ret;
        return -1;
    }

    return (clock_t)ret;
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

_sig_func_ptr signal(int sig, _sig_func_ptr handler)
{
    struct sigaction new;
    new.sa_handler = handler;
    new.sa_mask = 0;
    new.sa_flags = 0;

    struct sigaction old;
    int error = sigaction(sig, &new, &old);
    if(error < 0)
        return SIG_ERR;

    return old.sa_handler;
}

int sigaction(int signum, const struct sigaction* act, struct sigaction* oldact)
{
    int error = sys_sig_handler(signum, act, oldact);
    if(error < 0)
    {
        errno = -error;
        return -1;
    }

    return 0;
}

int link(char* old, char* new)
{
    UNIMPLEMENTED;
    errno = EMLINK;
    return -1;
}

DIR* opendir(const char* name)
{
    /* Open the file, if it doesn't exist: stop! */
    int descriptor = open(name, O_RDONLY);
    if(descriptor < 0)
    {
        errno = ENOENT;
        return NULL;
    }

    /* Create directory structure */
    DIR* dir = (DIR*)calloc(1, sizeof(DIR));
    dir->descriptor = descriptor;

    return dir;
}

struct dirent* readdir(DIR* dir)
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

int closedir(DIR* dir)
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
    //UNIMPLEMENTED;
    return;
}

void funlockfile(FILE* filehandle)
{
    //UNIMPLEMENTED;
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

int dup(int fd)
{
    int ret = sys_dup(fd);
    if(ret < 0)
    {
        errno = -ret;
        return -1;
    }

    return ret;
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

int chdir(const char* path)
{
    int ret = sys_chdir(path);
    if(ret < 0)
    {
        errno = -ret;
        return -1;
    }

    return 0;
}

int sched_yield(void)
{
    asm("int $0x81");
    return 0;
}

char* getcwd(char* buf, size_t size)
{
    /* If size is zero, default to PATH_MAX (4096) */
    size = (size <= 0) ? 4096 : size;

    /* Create buffer of size size if buf is not set */
    if(buf == NULL)
        buf = malloc(size + 1);

    sys_getcwd(buf, size);
    return buf;
}

char* get_current_dir_name(void)
{
    return getcwd(NULL, 0);
}

unsigned int sleep(unsigned int seconds)
{
    int ret = sys_sleep(seconds, 0);
    if(ret < 0)
    {
        errno = -ret;
        return -1;
    }

    return (unsigned int)ret;
}

int usleep(useconds_t usec)
{
    int ret = sys_sleep(0, usec);
    if(ret < 0)
    {
        errno = -ret;
        return -1;
    }

    return (unsigned int)ret;
}

int truncate(const char* path, off_t length)
{
    int ret = sys_truncate(path, length);
    if(ret < 0)
    {
        errno = -ret;
        return -1;
    }

    return ret;
}

int ftruncate(int fd, off_t length)
{
    int ret = sys_ftruncate(fd, length);
    if(ret < 0)
    {
        errno = -ret;
        return -1;
    }

    return ret;
}

uint32_t ntohl(uint32_t x)
{
    #if BYTE_ORDER == LITTLE_ENDIAN
        return __builtin_bswap32(x);
    #else
        return x;
    #endif
}

uint32_t htonl(uint32_t x)
{
    #if BYTE_ORDER == LITTLE_ENDIAN
        return __builtin_bswap32(x);
    #else
        return x;
    #endif
}

uint16_t ntohs(uint16_t x)
{
    #if BYTE_ORDER == LITTLE_ENDIAN
        return __builtin_bswap16(x);
    #else
        return x;
    #endif
}

uint16_t htons(uint16_t x)
{
    #if BYTE_ORDER == LITTLE_ENDIAN
        return __builtin_bswap16(x);
    #else
        return x;
    #endif
}