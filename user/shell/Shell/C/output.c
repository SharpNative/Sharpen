
struct class_Sharpen_IO_Console;
struct class_Sharpen_IO_Directory;
struct struct_Sharpen_IO_Directory_DirEntry;
struct class_Sharpen_Memory_Heap;
struct class_Shell_Sharpen_Memory_Mem;
struct class_Sharpen_Process;
struct class_Shell_Program;
struct class_Sharpen_Utilities_String;
struct class_Sharpen_Utilities_Util;


struct struct_Sharpen_IO_Directory_DirEntry
{
	uint32_t field_Ino;
	int32_t field_Offset;
	uint8_t field_Type;
	uint16_t field_Reclen;
	char field_Name[256];
};

struct class_Sharpen_IO_Console
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
} classStatics_Sharpen_IO_Console = {
};
struct class_Sharpen_IO_Directory
{
	int32_t usage_count;
	void** lookup_table;
	void* field_m_instance;
};

struct
{
} classStatics_Sharpen_IO_Directory = {
};
struct class_Sharpen_Memory_Heap
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
} classStatics_Sharpen_Memory_Heap = {
};
struct class_Shell_Sharpen_Memory_Mem
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
} classStatics_Shell_Sharpen_Memory_Mem = {
};
struct class_Sharpen_Process
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
} classStatics_Sharpen_Process = {
};
struct class_Shell_Program
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
} classStatics_Shell_Program = {
};
struct class_Sharpen_Utilities_String
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
} classStatics_Sharpen_Utilities_String = {
};
struct class_Sharpen_Utilities_Util
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
} classStatics_Sharpen_Utilities_Util = {
};

struct base_class
{
	int32_t usage_count;
	void** lookup_table;
};

struct class_Sharpen_IO_Console* classInit_Sharpen_IO_Console(void);
void Sharpen_IO_Console_Write_1char__(char* str);
typedef void (*fp_Sharpen_IO_Console_Write_1char__)(char* str);
void Sharpen_IO_Console_Write_1char_(char c);
typedef void (*fp_Sharpen_IO_Console_Write_1char_)(char c);
void Sharpen_IO_Console_WriteLine_1char__(char* str);
typedef void (*fp_Sharpen_IO_Console_WriteLine_1char__)(char* str);
char Sharpen_IO_Console_ReadChar_0(void);
typedef char (*fp_Sharpen_IO_Console_ReadChar_0)(void);
char* Sharpen_IO_Console_ReadLine_0(void);
typedef char* (*fp_Sharpen_IO_Console_ReadLine_0)(void);
void Sharpen_IO_Console_WriteHex_1int64_t_(int64_t num);
typedef void (*fp_Sharpen_IO_Console_WriteHex_1int64_t_)(int64_t num);
struct class_Sharpen_IO_Directory* classInit_Sharpen_IO_Directory(void);
static inline struct struct_Sharpen_IO_Directory_DirEntry structInit_Sharpen_IO_Directory_DirEntry(void);
void* Sharpen_IO_Directory_OpenInternal_1char__(char* path);
typedef void* (*fp_Sharpen_IO_Directory_OpenInternal_1char__)(char* path);
void Sharpen_IO_Directory_ReaddirInternal_3void__struct_struct_Sharpen_IO_Directory_DirEntry__uint32_t_(void* instance, struct struct_Sharpen_IO_Directory_DirEntry* entry, uint32_t index);
typedef void (*fp_Sharpen_IO_Directory_ReaddirInternal_3void__struct_struct_Sharpen_IO_Directory_DirEntry__uint32_t_)(void* instance, struct struct_Sharpen_IO_Directory_DirEntry* entry, uint32_t index);
void Sharpen_IO_Directory_CloseInternal_1void__(void* instance);
typedef void (*fp_Sharpen_IO_Directory_CloseInternal_1void__)(void* instance);
struct class_Sharpen_IO_Directory* Sharpen_IO_Directory_Open_1char__(char* path);
typedef struct class_Sharpen_IO_Directory* (*fp_Sharpen_IO_Directory_Open_1char__)(char* path);
struct struct_Sharpen_IO_Directory_DirEntry Sharpen_IO_Directory_Readdir_2class_uint32_t_(struct class_Sharpen_IO_Directory* obj, uint32_t index);
typedef struct struct_Sharpen_IO_Directory_DirEntry (*fp_Sharpen_IO_Directory_Readdir_2class_uint32_t_)(void* obj, uint32_t index);
void Sharpen_IO_Directory_Close_1class_(struct class_Sharpen_IO_Directory* obj);
typedef void (*fp_Sharpen_IO_Directory_Close_1class_)(void* obj);
int32_t Sharpen_IO_Directory_SetCurrentDirectory_1char__(char* path);
typedef int32_t (*fp_Sharpen_IO_Directory_SetCurrentDirectory_1char__)(char* path);
char* Sharpen_IO_Directory_GetCurrentDirectory_0(void);
typedef char* (*fp_Sharpen_IO_Directory_GetCurrentDirectory_0)(void);
struct class_Sharpen_Memory_Heap* classInit_Sharpen_Memory_Heap(void);
void* Sharpen_Memory_Heap_Alloc_1int32_t_(int32_t size);
typedef void* (*fp_Sharpen_Memory_Heap_Alloc_1int32_t_)(int32_t size);
void Sharpen_Memory_Heap_Free_1void__(void* ptr);
typedef void (*fp_Sharpen_Memory_Heap_Free_1void__)(void* ptr);
struct class_Shell_Sharpen_Memory_Mem* classInit_Shell_Sharpen_Memory_Mem(void);
void Shell_Sharpen_Memory_Mem_Memcpy_3void__void__int32_t_(void* destination, void* source, int32_t num);
typedef void (*fp_Shell_Sharpen_Memory_Mem_Memcpy_3void__void__int32_t_)(void* destination, void* source, int32_t num);
int32_t Shell_Sharpen_Memory_Mem_Compare_3char__char__int32_t_(char* s1, char* s2, int32_t n);
typedef int32_t (*fp_Shell_Sharpen_Memory_Mem_Compare_3char__char__int32_t_)(char* s1, char* s2, int32_t n);
void Shell_Sharpen_Memory_Mem_Memset_3void__int32_t_int32_t_(void* ptr, int32_t value, int32_t num);
typedef void (*fp_Shell_Sharpen_Memory_Mem_Memset_3void__int32_t_int32_t_)(void* ptr, int32_t value, int32_t num);
struct class_Sharpen_Process* classInit_Sharpen_Process(void);
int32_t Sharpen_Process_internalRun_2char__char___(char* path, char** args);
typedef int32_t (*fp_Sharpen_Process_internalRun_2char__char___)(char* path, char** args);
void Sharpen_Process_WaitForExit_1int32_t_(int32_t pid);
typedef void (*fp_Sharpen_Process_WaitForExit_1int32_t_)(int32_t pid);
void Sharpen_Process_Exit_1int32_t_(int32_t status);
typedef void (*fp_Sharpen_Process_Exit_1int32_t_)(int32_t status);
int32_t Sharpen_Process_Run_3char__char___int32_t_(char* path, char** argv, int32_t argc);
typedef int32_t (*fp_Sharpen_Process_Run_3char__char___int32_t_)(char* path, char** argv, int32_t argc);
struct class_Shell_Program* classInit_Shell_Program(void);
int32_t Shell_Program_TryRunFromExecDir_3char__char___int32_t_(char* name, char** argv, int32_t argc);
typedef int32_t (*fp_Shell_Program_TryRunFromExecDir_3char__char___int32_t_)(char* name, char** argv, int32_t argc);
void Shell_Program_Main_1char___(char** args);
typedef void (*fp_Shell_Program_Main_1char___)(char** args);
struct class_Sharpen_Utilities_String* classInit_Sharpen_Utilities_String(void);
int32_t Sharpen_Utilities_String_Length_1char__(char* text);
typedef int32_t (*fp_Sharpen_Utilities_String_Length_1char__)(char* text);
char* Sharpen_Utilities_String_Merge_2char__char__(char* first, char* second);
typedef char* (*fp_Sharpen_Utilities_String_Merge_2char__char__)(char* first, char* second);
int32_t Sharpen_Utilities_String_IndexOf_2char__char__(char* text, char* occurence);
typedef int32_t (*fp_Sharpen_Utilities_String_IndexOf_2char__char__)(char* text, char* occurence);
int32_t Sharpen_Utilities_String_Count_2char__char_(char* str, char occurence);
typedef int32_t (*fp_Sharpen_Utilities_String_Count_2char__char_)(char* str, char occurence);
char* Sharpen_Utilities_String_SubString_3char__int32_t_int32_t_(char* str, int32_t start, int32_t count);
typedef char* (*fp_Sharpen_Utilities_String_SubString_3char__int32_t_int32_t_)(char* str, int32_t start, int32_t count);
int32_t Sharpen_Utilities_String_Equals_2char__char__(char* one, char* two);
typedef int32_t (*fp_Sharpen_Utilities_String_Equals_2char__char__)(char* one, char* two);
char Sharpen_Utilities_String_ToUpper_1char_(char c);
typedef char (*fp_Sharpen_Utilities_String_ToUpper_1char_)(char c);
char Sharpen_Utilities_String_ToLower_1char_(char c);
typedef char (*fp_Sharpen_Utilities_String_ToLower_1char_)(char c);
struct class_Sharpen_Utilities_Util* classInit_Sharpen_Utilities_Util(void);
char* Sharpen_Utilities_Util_CharArrayToString_1char__(char* array);
typedef char* (*fp_Sharpen_Utilities_Util_CharArrayToString_1char__)(char* array);
char* Sharpen_Utilities_Util_CharPtrToString_1char__(char* ptr);
typedef char* (*fp_Sharpen_Utilities_Util_CharPtrToString_1char__)(char* ptr);
void* Sharpen_Utilities_Util_ObjectToVoidPtr_1object_t_(object_t obj);
typedef void* (*fp_Sharpen_Utilities_Util_ObjectToVoidPtr_1object_t_)(object_t obj);

static void* methods_Sharpen_IO_Console[];
static void* methods_Sharpen_IO_Directory[];
static void* methods_Sharpen_Memory_Heap[];
static void* methods_Shell_Sharpen_Memory_Mem[];
static void* methods_Sharpen_Process[];
static void* methods_Shell_Program[];
static void* methods_Sharpen_Utilities_String[];
static void* methods_Sharpen_Utilities_Util[];

struct class_Sharpen_IO_Console* classInit_Sharpen_IO_Console(void)
{
	struct class_Sharpen_IO_Console* object = calloc(1, sizeof(struct class_Sharpen_IO_Console));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_IO_Console;
	return object;
}

void Sharpen_IO_Console_WriteLine_1char__(char* str)
{
	Sharpen_IO_Console_Write_1char__(str);
	Sharpen_IO_Console_Write_1char__("\n");
}
char* Sharpen_IO_Console_ReadLine_0(void)
{
	char* buffer = calloc((1024), sizeof(char));
	char c;
	int32_t i = 0;
	while( ( c = Sharpen_IO_Console_ReadChar_0() )  != '\n')
	{
		{
			buffer[i] = c;
			i = i + 1;
			if(i > 1022)
						break;
		}
	}
	;
	buffer[i] = '\0';
	return Sharpen_Utilities_Util_CharArrayToString_1char__(buffer);
}
void Sharpen_IO_Console_WriteHex_1int64_t_(int64_t num)
{
	if(num == 0){
		Sharpen_IO_Console_Write_1char_('0');
		return;
	}
	int32_t noZeroes = true;
	for(int32_t j = 60;j >= 0;j -= 4)
	{
		{
			int64_t tmp =  ( num >> j )  & 0x0F;
			if(tmp == 0 && noZeroes)
						continue;
			noZeroes = false;
			if(tmp >= 0x0A){
				Sharpen_IO_Console_Write_1char_((char) ( tmp - 0x0A + 'A' ) );
			}
			else
			{
				Sharpen_IO_Console_Write_1char_((char) ( tmp + '0' ) );
			}
		}
	}
	;
}
struct class_Sharpen_IO_Directory* classInit_Sharpen_IO_Directory(void)
{
	struct class_Sharpen_IO_Directory* object = calloc(1, sizeof(struct class_Sharpen_IO_Directory));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_IO_Directory;
	return object;
}

static inline struct struct_Sharpen_IO_Directory_DirEntry structInit_Sharpen_IO_Directory_DirEntry(void)
{
	struct struct_Sharpen_IO_Directory_DirEntry object;
	return object;
}
struct class_Sharpen_IO_Directory* Sharpen_IO_Directory_Open_1char__(char* path)
{
	struct class_Sharpen_IO_Directory* instance = classInit_Sharpen_IO_Directory();
	instance->field_m_instance = Sharpen_IO_Directory_OpenInternal_1char__(path);
	return instance;
}
struct struct_Sharpen_IO_Directory_DirEntry Sharpen_IO_Directory_Readdir_2class_uint32_t_(struct class_Sharpen_IO_Directory* obj, uint32_t index)
{
	if(obj == NULL) fatal(__ERROR_NULL_CALLED__);
	struct struct_Sharpen_IO_Directory_DirEntry entry = structInit_Sharpen_IO_Directory_DirEntry();
	Sharpen_IO_Directory_ReaddirInternal_3void__struct_struct_Sharpen_IO_Directory_DirEntry__uint32_t_(obj->field_m_instance, &entry, index);
	return entry;
}
void Sharpen_IO_Directory_Close_1class_(struct class_Sharpen_IO_Directory* obj)
{
	if(obj == NULL) fatal(__ERROR_NULL_CALLED__);
	Sharpen_IO_Directory_CloseInternal_1void__(obj->field_m_instance);
}
struct class_Sharpen_Memory_Heap* classInit_Sharpen_Memory_Heap(void)
{
	struct class_Sharpen_Memory_Heap* object = calloc(1, sizeof(struct class_Sharpen_Memory_Heap));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Memory_Heap;
	return object;
}

struct class_Shell_Sharpen_Memory_Mem* classInit_Shell_Sharpen_Memory_Mem(void)
{
	struct class_Shell_Sharpen_Memory_Mem* object = calloc(1, sizeof(struct class_Shell_Sharpen_Memory_Mem));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Shell_Sharpen_Memory_Mem;
	return object;
}

int32_t Shell_Sharpen_Memory_Mem_Compare_3char__char__int32_t_(char* s1, char* s2, int32_t n)
{
	for(int32_t i = 0;i < n;i = i + 1
	)
	{
		{
			if(s1[i] != s2[i])
						return false;
		}
	}
	;
	return true;
}
struct class_Sharpen_Process* classInit_Sharpen_Process(void)
{
	struct class_Sharpen_Process* object = calloc(1, sizeof(struct class_Sharpen_Process));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Process;
	return object;
}

int32_t Sharpen_Process_Run_3char__char___int32_t_(char* path, char** argv, int32_t argc)
{
	char** args = calloc((argc + 1), sizeof(char*));
	if(argc > 0)
		for(int32_t i = 0;i < argc;i = i + 1
	)
	{
		args[i] = argv[i];
	}
	;
	args[argc] = null;
	return Sharpen_Process_internalRun_2char__char___(path, args);
}
struct class_Shell_Program* classInit_Shell_Program(void)
{
	struct class_Shell_Program* object = calloc(1, sizeof(struct class_Shell_Program));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Shell_Program;
	return object;
}

int32_t Shell_Program_TryRunFromExecDir_3char__char___int32_t_(char* name, char** argv, int32_t argc)
{
	char* total_string = Sharpen_Utilities_String_Merge_2char__char__("C://exec/", name);
	int32_t ret = Sharpen_Process_Run_3char__char___int32_t_(total_string, argv, argc);
	Sharpen_Memory_Heap_Free_1void__(Sharpen_Utilities_Util_ObjectToVoidPtr_1object_t_(total_string));
	return ret;
}
void Shell_Program_Main_1char___(char** args)
{
	Sharpen_IO_Console_WriteLine_1char__("Project Sharpen");
	Sharpen_IO_Console_WriteLine_1char__("(c) 2016 SharpNative\n");
	while(true)
	{
		{
			Sharpen_IO_Console_Write_1char__(Sharpen_IO_Directory_GetCurrentDirectory_0());
			Sharpen_IO_Console_Write_1char__("> ");
			char* read = Sharpen_IO_Console_ReadLine_0();
			int32_t offsetToSpace = Sharpen_Utilities_String_IndexOf_2char__char__(read, " ");
			if(offsetToSpace ==  - 1)
						offsetToSpace = Sharpen_Utilities_String_Length_1char__(read);
			char* command = Sharpen_Utilities_String_SubString_3char__int32_t_int32_t_(read, 0, offsetToSpace);
			if(command == null)
						continue;
			char** argv = null;
			int32_t argc = 1;
			if(read[offsetToSpace] == '\0'){
				argv = calloc((2), sizeof(char*));
				argv[0] = command;
				argv[1] = null;
			}
			else
			{
				char* argumentStart = Sharpen_Utilities_String_SubString_3char__int32_t_int32_t_(read, offsetToSpace + 1, Sharpen_Utilities_String_Length_1char__(read) - offsetToSpace - 1);
				argc = 1 +  ( Sharpen_Utilities_String_Count_2char__char_(argumentStart, ' ') + 1 ) ;
				argv = calloc((argc + 1), sizeof(char*));
				argv[0] = command;
				int32_t i = 0;
				int32_t offset = 0;
				for(;i < argc;i = i + 1
				)
				{
					{
						int32_t nextOffset = offset;
						for(;argumentStart[nextOffset] != ' ' && argumentStart[nextOffset] != '\0';nextOffset = nextOffset + 1
						)
						{
						}
						;
						char* arg = Sharpen_Utilities_String_SubString_3char__int32_t_int32_t_(argumentStart, offset, nextOffset - offset);
						offset = nextOffset + 1;
						argv[i + 1] = arg;
					}
				}
				;
				argv[i] = null;
			}
			if(Sharpen_Utilities_String_Equals_2char__char__(command, "cd")){
				if(argc != 2){
					Sharpen_IO_Console_WriteLine_1char__("Invalid usage of cd: cd [dirname]");
				}
				else
				{
					if( ! Sharpen_IO_Directory_SetCurrentDirectory_1char__(argv[1])){
						Sharpen_IO_Console_WriteLine_1char__("cd: Couldn't change the directory");
					}
				}
			}
			else if(Sharpen_Utilities_String_Equals_2char__char__(command, "dir")){
				struct class_Sharpen_IO_Directory* dir = Sharpen_IO_Directory_Open_1char__(Sharpen_IO_Directory_GetCurrentDirectory_0());
				uint32_t i = 0;
				while(true)
				{
					{
						struct struct_Sharpen_IO_Directory_DirEntry entry = Sharpen_IO_Directory_Readdir_2class_uint32_t_(dir, i);
						if(entry.field_Name[0] == '\0')
												break;
						char* str = Sharpen_Utilities_Util_CharPtrToString_1char__(entry.field_Name);
						Sharpen_IO_Console_WriteLine_1char__(str);
						i = i + 1
						;
					}
				}
				;
				Sharpen_IO_Directory_Close_1class_(dir);
			}
			else if(Sharpen_Utilities_String_Equals_2char__char__(command, "exit")){
				Sharpen_Process_Exit_1int32_t_(0);
			}
			else
			{
				int32_t ret = Sharpen_Process_Run_3char__char___int32_t_(command, argv, argc);
				if(ret < 0){
					ret = Shell_Program_TryRunFromExecDir_3char__char___int32_t_(command, argv, argc);
					if(ret < 0){
						Sharpen_IO_Console_Write_1char__(command);
						Sharpen_IO_Console_WriteLine_1char__(": Bad command or filename");
					}
				}
				Sharpen_Process_WaitForExit_1int32_t_(ret);
			}
		}
	}
	;
}
struct class_Sharpen_Utilities_String* classInit_Sharpen_Utilities_String(void)
{
	struct class_Sharpen_Utilities_String* object = calloc(1, sizeof(struct class_Sharpen_Utilities_String));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Utilities_String;
	return object;
}

int32_t Sharpen_Utilities_String_Length_1char__(char* text)
{
	int32_t i = 0;
	for(;text[i] != '\0';i = i + 1
	)
	{
	}
	;
	return i;
}
char* Sharpen_Utilities_String_Merge_2char__char__(char* first, char* second)
{
	int32_t firstLength = Sharpen_Utilities_String_Length_1char__(first);
	int32_t secondLength = Sharpen_Utilities_String_Length_1char__(second);
	int32_t totalLength = firstLength + secondLength;
	char* outVal = (char*)Sharpen_Memory_Heap_Alloc_1int32_t_(totalLength + 1);
	Shell_Sharpen_Memory_Mem_Memcpy_3void__void__int32_t_(outVal, Sharpen_Utilities_Util_ObjectToVoidPtr_1object_t_(first), firstLength);
	Shell_Sharpen_Memory_Mem_Memcpy_3void__void__int32_t_((void*) ( (int32_t)outVal + firstLength ) , Sharpen_Utilities_Util_ObjectToVoidPtr_1object_t_(second), secondLength);
	outVal[totalLength] = '\0';
	return Sharpen_Utilities_Util_CharPtrToString_1char__(outVal);
}
int32_t Sharpen_Utilities_String_IndexOf_2char__char__(char* text, char* occurence)
{
	int32_t found =  - 1;
	int32_t foundCount = 0;
	int32_t textLength = Sharpen_Utilities_String_Length_1char__(text);
	int32_t occurenceLength = Sharpen_Utilities_String_Length_1char__(occurence);
	if(textLength == 0 || occurenceLength == 0)
		return  - 1;
	for(int32_t textIndex = 0;textIndex < textLength;textIndex = textIndex + 1
	)
	{
		{
			if(occurence[foundCount] == text[textIndex]){
				if(foundCount == 0)
								found = textIndex;
				foundCount = foundCount + 1
				;
				if(foundCount >= occurenceLength)
								return found;
			}
			else
			{
				foundCount = 0;
				if(found >= 0)
								textIndex = found;
				found =  - 1;
			}
		}
	}
	;
	return  - 1;
}
int32_t Sharpen_Utilities_String_Count_2char__char_(char* str, char occurence)
{
	int32_t matches = 0;
	for(int32_t i = 0;str[i] != '\0';i = i + 1
	)
	{
		if(str[i] == occurence)
				matches = matches + 1
		;
	}
	;
	return matches;
}
char* Sharpen_Utilities_String_SubString_3char__int32_t_int32_t_(char* str, int32_t start, int32_t count)
{
	if(count <= 0)
		return null;
	int32_t stringLength = Sharpen_Utilities_String_Length_1char__(str);
	if(start > stringLength)
		return "";
	char* ch = (char*)Sharpen_Memory_Heap_Alloc_1int32_t_(count + 1);
	int32_t j = 0;
	for(int32_t i = start;j < count;i = i + 1
	)
	{
		{
			if(str[i] == '\0'){
				break;
			}
			ch[j] = str[i];
			j = j + 1;
		}
	}
	;
	ch[j] = '\0';
	return Sharpen_Utilities_Util_CharPtrToString_1char__(ch);
}
int32_t Sharpen_Utilities_String_Equals_2char__char__(char* one, char* two)
{
	int32_t oneLength = Sharpen_Utilities_String_Length_1char__(one);
	int32_t twoLength = Sharpen_Utilities_String_Length_1char__(two);
	if(oneLength != twoLength)
		return false;
	for(int32_t i = 0;i < oneLength;i = i + 1
	)
	{
		if(one[i] != two[i])
				return false;
	}
	;
	return true;
}
char Sharpen_Utilities_String_ToUpper_1char_(char c)
{
	return  ( c >= 'a' && c <= 'z' )  ? (char) ( c +  ( 'A' - 'a' )  )  : c;
}
char Sharpen_Utilities_String_ToLower_1char_(char c)
{
	return  ( c >= 'A' && c <= 'Z' )  ? (char) ( c + 32 )  : c;
}
struct class_Sharpen_Utilities_Util* classInit_Sharpen_Utilities_Util(void)
{
	struct class_Sharpen_Utilities_Util* object = calloc(1, sizeof(struct class_Sharpen_Utilities_Util));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Sharpen_Utilities_Util;
	return object;
}


static void* methods_Sharpen_IO_Console[] = {Sharpen_IO_Console_Write_1char__,Sharpen_IO_Console_Write_1char_,Sharpen_IO_Console_WriteLine_1char__,Sharpen_IO_Console_ReadChar_0,Sharpen_IO_Console_ReadLine_0,Sharpen_IO_Console_WriteHex_1int64_t_,};
static void* methods_Sharpen_IO_Directory[] = {Sharpen_IO_Directory_OpenInternal_1char__,Sharpen_IO_Directory_ReaddirInternal_3void__struct_struct_Sharpen_IO_Directory_DirEntry__uint32_t_,Sharpen_IO_Directory_CloseInternal_1void__,Sharpen_IO_Directory_Open_1char__,Sharpen_IO_Directory_Readdir_2class_uint32_t_,Sharpen_IO_Directory_Close_1class_,Sharpen_IO_Directory_SetCurrentDirectory_1char__,Sharpen_IO_Directory_GetCurrentDirectory_0,};
static void* methods_Sharpen_Memory_Heap[] = {Sharpen_Memory_Heap_Alloc_1int32_t_,Sharpen_Memory_Heap_Free_1void__,};
static void* methods_Shell_Sharpen_Memory_Mem[] = {Shell_Sharpen_Memory_Mem_Memcpy_3void__void__int32_t_,Shell_Sharpen_Memory_Mem_Compare_3char__char__int32_t_,Shell_Sharpen_Memory_Mem_Memset_3void__int32_t_int32_t_,};
static void* methods_Sharpen_Process[] = {Sharpen_Process_internalRun_2char__char___,Sharpen_Process_WaitForExit_1int32_t_,Sharpen_Process_Exit_1int32_t_,Sharpen_Process_Run_3char__char___int32_t_,};
static void* methods_Shell_Program[] = {Shell_Program_TryRunFromExecDir_3char__char___int32_t_,Shell_Program_Main_1char___,};
static void* methods_Sharpen_Utilities_String[] = {Sharpen_Utilities_String_Length_1char__,Sharpen_Utilities_String_Merge_2char__char__,Sharpen_Utilities_String_IndexOf_2char__char__,Sharpen_Utilities_String_Count_2char__char_,Sharpen_Utilities_String_SubString_3char__int32_t_int32_t_,Sharpen_Utilities_String_Equals_2char__char__,Sharpen_Utilities_String_ToUpper_1char_,Sharpen_Utilities_String_ToLower_1char_,};
static void* methods_Sharpen_Utilities_Util[] = {Sharpen_Utilities_Util_CharArrayToString_1char__,Sharpen_Utilities_Util_CharPtrToString_1char__,Sharpen_Utilities_Util_ObjectToVoidPtr_1object_t_,};

void init(void)
{
}
