#include <stdio.h>
#include <stdint.h>
#include <stdlib.h>
#include <malloc.h>
#include <fcntl.h>
#include <dirent.h>
#include <string.h>
#include <unistd.h>
#include <sys/wait.h>

/* Values */
#define true (1)
#define false (0)
#define null ((void*) 0)

/* Types */
typedef void* action_t;
typedef void* object_t;
typedef int32_t bool_t;
typedef char* string_t;

/* Error messages */
const static char* __ERROR_NULL_CALLED__ = "The program tried to call a method of an object that is null";

char* get_current_dir_name(void);

void fatal(const char* s)
{
    perror(s);
    abort();
}

inline char* Sharpen_Utilities_Util_CharArrayToString_1char__(char* array)
{
    return array;
}

inline char* Sharpen_Utilities_Util_CharPtrToString_1char__(char* ptr)
{
    return ptr;
}

void Shell_Sharpen_Memory_Mem_Memcpy_3void__void__int32_t_(void* destination, void* source, int32_t num)
{
    memcpy(destination, source, num);
}
void Shell_Sharpen_Memory_Mem_Memset_3void__int32_t_int32_t_(void* ptr, int32_t value, int32_t num)
{
    memset(ptr, value, num);
}

#include "Shell/C/output.c"

int main(int argc, char* argv[])
{
    init();
    Shell_Program_Main_1string_t__((char**)argv);
    return 0;
}

void Sharpen_IO_Console_Write_1string_t_(string_t str)
{
    printf("%s", str);
    fflush(stdout);
}

void Sharpen_IO_Console_Write_1char_(char c)
{
    putchar(c);
    fflush(stdout);
}

char Sharpen_IO_Console_ReadChar_0(void)
{
    return fgetc(stdin);
}

int run(char*, char**, char**);

int32_t Sharpen_Process_internalRun_2string_t_string_t__(string_t path, string_t* args)
{
    return run(path, args, NULL);
}

void Sharpen_Process_WaitForExit_1int32_t_(int32_t pid)
{
    waitpid((pid_t)pid, NULL, 0);
}

void Sharpen_Process_Exit_1int32_t_(int32_t status)
{
    _exit(status);
}

inline void *Sharpen_Utilities_Util_ObjectToVoidPtr_1object_t_(object_t ptr)
{
    return ptr;
}


void* Sharpen_Memory_Heap_Alloc_1int32_t_(int32_t size)
{
    return malloc(size);
}

void Sharpen_Memory_Heap_Free_1void__(void* ptr)
{
    free(ptr);
}

void Sharpen_Memory_Heap_Free_1object_t_(object_t obj)
{
    free(obj);
}


void* Sharpen_IO_Directory_OpenInternal_1string_t_(string_t path)
{
    return opendir(path);
}

void Sharpen_IO_Directory_ReaddirInternal_3void__struct_struct_Sharpen_IO_Directory_DirEntry__uint32_t_(void* instance, struct struct_Sharpen_IO_Directory_DirEntry* entry, uint32_t index)
{
    DIR* dir = (DIR*)instance;
    dir->last = index;
    (void)readdir(dir);
    memcpy(entry, &dir->__current, sizeof(struct dirent));
}

void Sharpen_IO_Directory_CloseInternal_1void__(void* instance)
{
    closedir(instance);
}


int32_t Sharpen_IO_Directory_SetCurrentDirectory_1string_t_(string_t path)
{
    return (chdir(path) == 0) ? 1 : 0;
}

string_t Sharpen_IO_Directory_GetCurrentDirectory_0(void)
{
    return get_current_dir_name();
}