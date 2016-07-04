#include <heap.h>

/** TODO: implement this **/


static void* heap_start;

/**
 *
 * Sets the start address of the heap
 * @param start the start address
 *
**/
void Sharpen_Heap_Init_1(void* start)
{
	heap_start = start;
}

/**
 *
 * Allocates a piece of memory on the heap
 * @param  size the size
 * @return the pointer
 *
**/
void* Sharpen_Heap_Alloc_1(int32_t size)
{
	void* ret = heap_start;
	heap_start = (void*) ((int) heap_start + size);
	return ret;
}

/**
 *
 * Allocates a piece of aligned memory on the heap
 * @param  alignment the alignment
 * @param  size the size
 * @return the pointer
 *
**/
void* Sharpen_Heap_AlignedAlloc_2(int32_t alignment, int32_t size)
{
	(void) alignment;
	return Sharpen_Heap_Alloc_1(size);
}

/**
 *
 * Frees a piece of code
 * @param ptr the pointer
 *
**/
void Sharpen_Heap_Free_1(void* ptr)
{
	(void) ptr;
}