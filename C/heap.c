#include <stdint.h>

void* ptr = (void*) 0x911000;


void* malloc(size_t size)
{
	void* ret = ptr;
	ptr = (void*) ((int) ptr + size);
	return ret;
}