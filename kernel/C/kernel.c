#include <stdint.h>

/* Constants */
#define NULL ((void*)0)
#define SSE_XMM_SIZE 16

/* Prototypes */
static void* calloc(int nitems, int size);
void Sharpen_Mem_Memory_Memcpy_3void__void__int32_t_(void* destination, void* source, int32_t num);
void Sharpen_Mem_Memory_Memset_3void__int32_t_int32_t_(void* ptr, int32_t value, int32_t num);
void Sharpen_Mem_Memory_Memclear_2void__int32_t_(void* ptr, int32_t num);

/* Both memcpy and memset are here because the compiler may decide if it wants to optimize something */
/* using these methods */

inline void* memcpy(void* destination, void* source, int num)
{
    Sharpen_Mem_Memory_Memcpy_3void__void__int32_t_(destination, source, num);
    return destination;
}

inline void* memset(void* ptr, int value, int num)
{
    if(value == 0)
        Sharpen_Mem_Memory_Memclear_2void__int32_t_(ptr, num);
    else
        Sharpen_Mem_Memory_Memset_3void__int32_t_int32_t_(ptr, value, num);

    return ptr;
}

/* Adapted from http://wiki.osdev.org/User:01000101/optlib/#non-SSE_.28standard.29_version */
/* License: public domain */
void Sharpen_Mem_Memory_Memclear_2void__int32_t_(void* ptr, int num)
{
    /* Align on an SSE XMM boundary */
    int32_t i = ((intptr_t)ptr + (SSE_XMM_SIZE - 1)) & ~(SSE_XMM_SIZE - 1);
    i -= (intptr_t)ptr;

    if(i > num)
        i = num;

    /* Memset the part that cannot be copied using SSE because it is not aligned */
    if(i > 0)
    {
        Sharpen_Mem_Memory_Memset_3void__int32_t_int32_t_(ptr, 0, i);
    }

    /* Clear 64-byte chunks of memory (4 times 16 bytes) */
    for(; i + 64 <= num; i += 64)
    {
        __asm__ __volatile__("pxor %%xmm0, %%xmm0;"   /* Set XMM0 to zero */
                             "movdqa %%xmm0, 0(%0);"  /* Move 16 bytes from XMM0 to %0 + 0 */
                             "movdqa %%xmm0, 16(%0);" /* Move 16 bytes from XMM0 to %0 + 16 */
                             "movdqa %%xmm0, 32(%0);" /* Move 16 bytes from XMM0 to %0 + 32 */
                             "movdqa %%xmm0, 48(%0);" /* Move 16 bytes from XMM0 to %0 + 48 */
                             :: "r"((int)ptr + i)
                            );
    }

    /* Memset the remaining bytes */
    Sharpen_Mem_Memory_Memset_3void__int32_t_int32_t_((void*)((intptr_t)ptr + i), 0, num - i);
}

/* Macro to methods */
#define malloc      Sharpen_Mem_Heap_Alloc_1int32_t_
#define free        Sharpen_Mem_Heap_Free_1void__
#define fatal(msg)  Sharpen_Panic_DoPanic_1string_t_((char*)msg)

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

inline string_t Sharpen_Utilities_Util_CharPtrToString_1char__(char* ptr)
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

inline object_t Sharpen_Utilities_Util_VoidPtrToObject_1void__(void* ptr)
{
    return ptr;
}

inline void* Sharpen_Utilities_Util_MethodToPtr_1action_t_(action_t ptr)
{
    return ptr;
}

inline void Sharpen_Utilities_Util_WriteVolatile32_2uint32_t_uint32_t_(uint32_t address, uint32_t value)
{
    *(volatile uint32_t volatile*)address = value;
}

inline uint32_t Sharpen_Utilities_Util_ReadVolatile32_1uint32_t_(uint32_t address)
{
    return *(volatile uint32_t volatile*)address;
}

inline void* Sharpen_SymbolTable_getSymbolTable_0(void)
{
    extern void* kernelsymbols;
    return (void*)&kernelsymbols;
}

inline void* Sharpen_Arch_Paging_getUsercodeAddress_0(void)
{
    extern void* usercode;
    return (void*)&usercode;
}