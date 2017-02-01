#include <stdio.h>
#include <stdint.h>
#include <stdlib.h>
#include <malloc.h>
#include <fcntl.h>
#include <dirent.h>
#include <string.h>
#include <unistd.h>
#include <sys/wait.h>

/* Types */
typedef void* action_t;
typedef void* object_t;
typedef int32_t bool_t;
typedef char* string_t;

/* Error messages */
const static char* __ERROR_NULL_CALLED__ = "The program tried to call a method of an object that is null";

int run(char*, char**, char**);

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

inline void* Sharpen_Utilities_Util_ObjectToVoidPtr_1object_t_(object_t ptr)
{
    return ptr;
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

int32_t Sharpen_IO_Directory_SetCurrentDirectory_1string_t_(string_t path)
{
    return (chdir(path) == 0) ? 1 : 0;
}

#include "Shell/C/output.c"

int main(int argc, char* argv[])
{
    init();
    Shell_Program_Main_1string_t__((char**)argv);
    return 0;
}