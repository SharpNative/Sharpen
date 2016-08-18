#include <stdint.h>

/* Values */
#define true (1)
#define false (0)
#define NULL ((void*) 0)
#define null ((void*) 0)

/* Memory */
#define malloc  Sharpen_Heap_Alloc_1int32_t_
#define free    Sharpen_Heap_Free_1void__
#define memcpy  Sharpen_Memory_Memcpy_3void__void__int32_t_
#define memset  Sharpen_Memory_Memset_3void__int32_t_int32_t_

void* calloc(int nitems, int size);

/* Compiled C file */
#include "output.c"

void* calloc(int nitems, int size)
{
	void* ptr = malloc(nitems * size);
	if(ptr == NULL)
		return NULL;
	memset(ptr, 0, nitems * size);
	return ptr;
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

inline void* Sharpen_Utilities_Util_ObjectToVoidPtr_1void__(void* ptr)
{
	return ptr;
}

inline void* Sharpen_Utilities_Util_MethodToPtr_1void__(void* ptr)
{
	return ptr;
}