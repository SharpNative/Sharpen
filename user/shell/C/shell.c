#include <stdio.h>
#include <stdint.h>
#include <malloc.h>
#include <fcntl.h>

#define true (1)
#define false (0)
#define null NULL

void Shell_Console_Write_1char__(char* str)
{
	printf("%s", str);
	fflush(stdout);
}

void Shell_Console_Write_1char_(char c)
{
	putchar(c);
	fflush(stdout);
}

char Shell_Console_ReadChar_0(void)
{
	return fgetc(stdin);
}

inline char* Shell_Util_CharArrayToString_1char__(char* array)
{
	return array;
}

inline char* Shell_Util_CharPtrToString_1char__(char* ptr)
{
	return ptr;
}


void* Shell_Heap_Alloc_1int32_t_(int32_t size)
{
	return malloc(size);
}

void Shell_Heap_Free_1void__(void* ptr)
{
	return free(ptr);
}

#include "output.c"

int main(int argc, char* argv[])
{
	(void) open("devices://keyboard", O_RDONLY);
    (void) open("devices://stdout",   O_WRONLY);
    (void) open("devices://stdout",   O_WRONLY);

	init();
	Shell_Program_Main_1char___((void*)argv);
	return 0;
}