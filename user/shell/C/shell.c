#include <stdio.h>
#include <stdint.h>
#include <malloc.h>
#include <fcntl.h>
#include <dirent.h>
#include <string.h>
#include <unistd.h>
#include <sys/wait.h>

#define true (1)
#define false (0)
#define null NULL

inline char* Shell_Util_CharArrayToString_1char__(char* array)
{
	return array;
}

inline char* Shell_Util_CharPtrToString_1char__(char* ptr)
{
	return ptr;
}



#include "output.c"

int main(int argc, char* argv[])
{
	init();
	Shell_Program_Main_1char___((void*)argv);
	return 0;
}

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

int run(char*, char**, char**);

int32_t Shell_Process_internalRun_2char__char___(char* path, char** args)
{
	return run(path, args, NULL);
}

void Shell_Process_WaitForExit_1int32_t_(int32_t pid)
{
	waitpid((pid_t)pid, NULL, 0);
}

void Shell_Process_Exit_1int32_t_(int32_t status)
{
	_exit(status);
}

inline void *Shell_Util_ObjectToVoidPtr_1void__(void* ptr)
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


void* Shell_Directory_OpenInternal_1char__(char* path)
{
	return opendir(path);
}

void Shell_Directory_ReaddirInternal_3void__struct_struct_Shell_Directory_DirEntry__uint32_t_(void* instance, struct struct_Shell_Directory_DirEntry* entry, uint32_t index)
{
	DIR* dir = (DIR*)instance;
	dir->last = index;
	(void)readdir(dir);
	memcpy(entry, &dir->__current, sizeof(struct dirent));
}

void Shell_Directory_CloseInternal_1void__(void* instance)
{
	closedir(instance);
}