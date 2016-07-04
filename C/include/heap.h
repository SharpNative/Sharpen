#ifndef __HEAP_H
#define __HEAP_H

#include <stdint.h>

void  Sharpen_Heap_Init(void* start);
void *Sharpen_Heap_AlignedAlloc(int32_t alignment, int32_t size);
void *Sharpen_Heap_Alloc(int32_t size);
void  Sharpen_Heap_Free(void* ptr);

#endif