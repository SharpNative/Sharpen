#include <stdlib.h>

/* Values */
#define true (1)
#define false (0)

/* Memory */
#define malloc  Sharpen_Heap_Alloc_1int32_t_
#define free    Sharpen_Heap_Free_1void__
#define memcpy  Sharpen_Memory_Memcpy_3void__void__int32_t_
#define memset  Sharpen_Memory_Memset_3void__int32_t_int32_t_

/* Compiled C file */
#include "output.c"