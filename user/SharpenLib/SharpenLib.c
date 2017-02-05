#include "header.h"

void init(void);

void fatal(const char* s)
{
    perror(s);
    abort();
}

/* Compiled C code */
#include "SharpenLib/C/output.c"

inline char* Sharpen_Utilities_Util_CharArrayToString_1char__(char* array)
{
    return array;
}

inline string_t Sharpen_Utilities_Util_CharPtrToString_1char__(char* ptr)
{
    return ptr;
}

inline void* Sharpen_Utilities_Util_ObjectToVoidPtr_1object_t_(object_t ptr)
{
    return ptr;
}

inline void Sharpen_IO_Console_Flush_0(void)
{
    fflush(stdout);
}

void Sharpen_IO_Console_Write_1string_t_(string_t str)
{
    printf("%s", str);
    fflush(stdout);
}

char Sharpen_IO_Console_ReadChar_0(void)
{
    return fgetc(stdin);
}

int main(int argc, char* argv[])
{
    init_SharpenLib();
    init();
    EntryPoint((char**)argv);
    return 0;
}