#include <stdlib.h>

/* Values */
#define true (1)
#define false (0)

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
	memset(ptr, 0, nitems * size);
	return ptr;
}