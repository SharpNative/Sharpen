#include <stdint.h>

/* Values */
#define true (1)
#define false (0)
#define NULL ((void*) 0)
#define null ((void*) 0)

/* Types */
typedef void*   action_t;
typedef void*   object_t;
typedef int32_t bool_t;
typedef char* string_t;

/* Macro to methods */
#define malloc      Sharpen_Mem_Heap_Alloc_1int32_t_
#define free        Sharpen_Mem_Heap_Free_1void__
#define memcpy      Sharpen_Mem_Memory_Memcpy_3void__void__int32_t_
#define memset      Sharpen_Mem_Memory_Memset_3void__int32_t_int32_t_
#define fatal(msg)  __abort(__FUNCTION__, __LINE__, msg)

static void* calloc(int nitems, int size);
static void __abort(const char* function, int line, const char* msg);

/* Error messages */
const static char* __ERROR_NULL_CALLED__ = "The program tried to call a method of an object that is null";

/* Compiled C file */
#include "output.c"

static void* calloc(int nitems, int size)
{
    void* ptr = malloc(nitems * size);
    if(ptr == NULL)
        return NULL;
    memset(ptr, 0, nitems * size);
    return ptr;
}

static void __abort(const char* function, int line, const char* msg)
{
    Sharpen_Console_Write_1string_t_("\tABORT\n");
    Sharpen_Console_Write_1string_t_("\tFunction: ");
    Sharpen_Console_Write_1string_t_((string_t)function);
    Sharpen_Console_Write_1string_t_(" | Line: ");
    Sharpen_Console_WriteNum_1int32_t_(line);
    Sharpen_Console_Write_1char_('\n');
    Sharpen_Panic_DoPanic_1string_t_((string_t)msg);
}

inline char* Sharpen_Utilities_Util_CharPtrToString_1char__(char* ptr)
{
    return ptr;
}

inline uint8_t* Sharpen_Utilities_Util_PtrToArray_1uint8_t__(uint8_t* ptr)
{
    return ptr;
}

inline void** Sharpen_Utilities_Util_PtrToArray_1void__(void* ptr)
{
    return ptr;
}

inline char** Sharpen_Utilities_Util_PtrToArray_1char___(char** ptr)
{
    return ptr;
}

inline void* Sharpen_Utilities_Util_ObjectToVoidPtr_1object_t_(object_t ptr)
{
    return ptr;
}

inline void* Sharpen_Utilities_Util_VoidPtrToObject_1void__(void* ptr)
{
    return ptr;
}

inline void* Sharpen_Utilities_Util_MethodToPtr_1action_t_(action_t ptr)
{
    return ptr;
}