
struct class_Shell_Console;
struct class_Shell_Directory;
struct struct_Shell_Directory_DirEntry;
struct class_Shell_Heap;
struct class_Shell_Process;
struct class_Shell_Program;
struct class_Shell_String;
struct class_Shell_Util;


struct struct_Shell_Directory_DirEntry
{
	uint32_t field_Ino;
	int32_t field_Offset;
	uint8_t field_Type;
	uint16_t field_Reclen;
	char field_Name[256];
};

struct class_Shell_Console
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
} classStatics_Shell_Console = {
};
struct class_Shell_Directory
{
	int32_t usage_count;
	void** lookup_table;
	void* field_m_instance;
};

struct
{
} classStatics_Shell_Directory = {
};
struct class_Shell_Heap
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
} classStatics_Shell_Heap = {
};
struct class_Shell_Process
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
} classStatics_Shell_Process = {
};
struct class_Shell_Program
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
	char* m_folder;
} classStatics_Shell_Program = {
	"C://",
};
struct class_Shell_String
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
} classStatics_Shell_String = {
};
struct class_Shell_Util
{
	int32_t usage_count;
	void** lookup_table;
};

struct
{
} classStatics_Shell_Util = {
};

struct base_class
{
	int32_t usage_count;
	void** lookup_table;
};

struct class_Shell_Console* classInit_Shell_Console(void);
void Shell_Console_Write_1char__(char* str);
typedef void (*fp_Shell_Console_Write_1char__)(char* str);
void Shell_Console_Write_1char_(char c);
typedef void (*fp_Shell_Console_Write_1char_)(char c);
void Shell_Console_WriteLine_1char__(char* str);
typedef void (*fp_Shell_Console_WriteLine_1char__)(char* str);
char Shell_Console_ReadChar_0(void);
typedef char (*fp_Shell_Console_ReadChar_0)(void);
char* Shell_Console_ReadLine_0(void);
typedef char* (*fp_Shell_Console_ReadLine_0)(void);
void Shell_Console_WriteHex_1int64_t_(int64_t num);
typedef void (*fp_Shell_Console_WriteHex_1int64_t_)(int64_t num);
struct class_Shell_Directory* classInit_Shell_Directory(void);
inline struct struct_Shell_Directory_DirEntry structInit_Shell_Directory_DirEntry(void);
void* Shell_Directory_OpenInternal_1char__(char* path);
typedef void* (*fp_Shell_Directory_OpenInternal_1char__)(char* path);
void Shell_Directory_ReaddirInternal_3void__struct_struct_Shell_Directory_DirEntry__uint32_t_(void* instance, struct struct_Shell_Directory_DirEntry* entry, uint32_t index);
typedef void (*fp_Shell_Directory_ReaddirInternal_3void__struct_struct_Shell_Directory_DirEntry__uint32_t_)(void* instance, struct struct_Shell_Directory_DirEntry* entry, uint32_t index);
void Shell_Directory_CloseInternal_1void__(void* instance);
typedef void (*fp_Shell_Directory_CloseInternal_1void__)(void* instance);
struct class_Shell_Directory* Shell_Directory_Open_1char__(char* path);
typedef struct class_Shell_Directory* (*fp_Shell_Directory_Open_1char__)(char* path);
struct struct_Shell_Directory_DirEntry Shell_Directory_Readdir_2class_uint32_t_(struct class_Shell_Directory* obj, uint32_t index);
typedef struct struct_Shell_Directory_DirEntry (*fp_Shell_Directory_Readdir_2class_uint32_t_)(void* obj, uint32_t index);
void Shell_Directory_Close_1class_(struct class_Shell_Directory* obj);
typedef void (*fp_Shell_Directory_Close_1class_)(void* obj);
struct class_Shell_Heap* classInit_Shell_Heap(void);
void* Shell_Heap_Alloc_1int32_t_(int32_t size);
typedef void* (*fp_Shell_Heap_Alloc_1int32_t_)(int32_t size);
void Shell_Heap_Free_1void__(void* ptr);
typedef void (*fp_Shell_Heap_Free_1void__)(void* ptr);
struct class_Shell_Process* classInit_Shell_Process(void);
void Shell_Process_internalRun_2char__char___(char* path, char** args);
typedef void (*fp_Shell_Process_internalRun_2char__char___)(char* path, char** args);
void Shell_Process_Run_3char__char___int32_t_(char* path, char** argv, int32_t argc);
typedef void (*fp_Shell_Process_Run_3char__char___int32_t_)(char* path, char** argv, int32_t argc);
struct class_Shell_Program* classInit_Shell_Program(void);
inline void classCctor_Shell_Program(void);
void Shell_Program_Main_1char___(char** args);
typedef void (*fp_Shell_Program_Main_1char___)(char** args);
struct class_Shell_String* classInit_Shell_String(void);
int32_t Shell_String_Length_1char__(char* text);
typedef int32_t (*fp_Shell_String_Length_1char__)(char* text);
int32_t Shell_String_IndexOf_2char__char__(char* text, char* occurence);
typedef int32_t (*fp_Shell_String_IndexOf_2char__char__)(char* text, char* occurence);
char* Shell_String_Merge_2char__char__(char* first, char* second);
typedef char* (*fp_Shell_String_Merge_2char__char__)(char* first, char* second);
int32_t Shell_String_Count_2char__char_(char* str, char occurence);
typedef int32_t (*fp_Shell_String_Count_2char__char_)(char* str, char occurence);
char* Shell_String_SubString_3char__int32_t_int32_t_(char* str, int32_t start, int32_t count);
typedef char* (*fp_Shell_String_SubString_3char__int32_t_int32_t_)(char* str, int32_t start, int32_t count);
int32_t Shell_String_Equals_2char__char__(char* one, char* two);
typedef int32_t (*fp_Shell_String_Equals_2char__char__)(char* one, char* two);
char Shell_String_ToUpper_1char_(char c);
typedef char (*fp_Shell_String_ToUpper_1char_)(char c);
char Shell_String_ToLower_1char_(char c);
typedef char (*fp_Shell_String_ToLower_1char_)(char c);
struct class_Shell_Util* classInit_Shell_Util(void);
char* Shell_Util_CharArrayToString_1char__(char* array);
typedef char* (*fp_Shell_Util_CharArrayToString_1char__)(char* array);
char* Shell_Util_CharPtrToString_1char__(char* ptr);
typedef char* (*fp_Shell_Util_CharPtrToString_1char__)(char* ptr);
void* Shell_Util_ObjectToVoidPtr_1void__(void* obj);
typedef void* (*fp_Shell_Util_ObjectToVoidPtr_1void__)(void* obj);

static void* methods_Shell_Console[];
static void* methods_Shell_Directory[];
static void* methods_Shell_Heap[];
static void* methods_Shell_Process[];
static void* methods_Shell_Program[];
static void* methods_Shell_String[];
static void* methods_Shell_Util[];

struct class_Shell_Console* classInit_Shell_Console(void)
{
	struct class_Shell_Console* object = calloc(1, sizeof(struct class_Shell_Console));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Shell_Console;
	return object;
}

void Shell_Console_WriteLine_1char__(char* str)
{
	Shell_Console_Write_1char__(str);
	Shell_Console_Write_1char__("\n");
}
char* Shell_Console_ReadLine_0(void)
{
	char* buffer = calloc((1024), sizeof(char));
	char c;
	int32_t i = 0;
	while( ( c = Shell_Console_ReadChar_0() )  != '\n')
	{
		{
			buffer[i] = c;
			i = i + 1;
			Shell_Console_Write_1char_(c);
			if(i > 1022)
						break;
		}
	}
	;
	buffer[i] = '\0';
	return Shell_Util_CharArrayToString_1char__(buffer);
}
void Shell_Console_WriteHex_1int64_t_(int64_t num)
{
	if(num == 0){
		Shell_Console_Write_1char_('0');
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
				Shell_Console_Write_1char_((char) ( tmp - 0x0A + 'A' ) );
			}
			else
			{
				Shell_Console_Write_1char_((char) ( tmp + '0' ) );
			}
		}
	}
	;
}
struct class_Shell_Directory* classInit_Shell_Directory(void)
{
	struct class_Shell_Directory* object = calloc(1, sizeof(struct class_Shell_Directory));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Shell_Directory;
	return object;
}

inline struct struct_Shell_Directory_DirEntry structInit_Shell_Directory_DirEntry(void)
{
	struct struct_Shell_Directory_DirEntry object;
	return object;
}
struct class_Shell_Directory* Shell_Directory_Open_1char__(char* path)
{
	struct class_Shell_Directory* instance = classInit_Shell_Directory();
	instance->field_m_instance = Shell_Directory_OpenInternal_1char__(path);
	return instance;
}
struct struct_Shell_Directory_DirEntry Shell_Directory_Readdir_2class_uint32_t_(struct class_Shell_Directory* obj, uint32_t index)
{
	struct struct_Shell_Directory_DirEntry entry = structInit_Shell_Directory_DirEntry();
	Shell_Directory_ReaddirInternal_3void__struct_struct_Shell_Directory_DirEntry__uint32_t_(obj->field_m_instance, &entry, index);
	return entry;
}
void Shell_Directory_Close_1class_(struct class_Shell_Directory* obj)
{
	Shell_Directory_CloseInternal_1void__(obj->field_m_instance);
}
struct class_Shell_Heap* classInit_Shell_Heap(void)
{
	struct class_Shell_Heap* object = calloc(1, sizeof(struct class_Shell_Heap));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Shell_Heap;
	return object;
}

struct class_Shell_Process* classInit_Shell_Process(void)
{
	struct class_Shell_Process* object = calloc(1, sizeof(struct class_Shell_Process));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Shell_Process;
	return object;
}

void Shell_Process_Run_3char__char___int32_t_(char* path, char** argv, int32_t argc)
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
	Shell_Process_internalRun_2char__char___(path, args);
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

inline void classCctor_Shell_Program(void)
{
}
void Shell_Program_Main_1char___(char** args)
{
	while(true)
	{
		{
			Shell_Console_Write_1char__(classStatics_Shell_Program.m_folder);
			Shell_Console_Write_1char__("> ");
			char* read = Shell_Console_ReadLine_0();
			Shell_Console_WriteLine_1char__("");
			int32_t offsetToSpace = Shell_String_IndexOf_2char__char__(read, " ");
			if(offsetToSpace ==  - 1)
						offsetToSpace = Shell_String_Length_1char__(read);
			char* command = Shell_String_SubString_3char__int32_t_int32_t_(read, 0, offsetToSpace);
			if(command == null)
						continue;
			if(Shell_String_Equals_2char__char__(command, "cd")){
			}
			else if(Shell_String_Equals_2char__char__(command, "dir")){
				struct class_Shell_Directory* dir = Shell_Directory_Open_1char__(classStatics_Shell_Program.m_folder);
				uint32_t i = 0;
				while(true)
				{
					{
						struct struct_Shell_Directory_DirEntry entry = Shell_Directory_Readdir_2class_uint32_t_(dir, i);
						if(entry.field_Name[0] == (char)0x00)
												break;
						char* str = Shell_Util_CharPtrToString_1char__(entry.field_Name);
						Shell_Console_WriteLine_1char__(str);
						i = i + 1
						;
					}
				}
				;
				Shell_Directory_Close_1class_(dir);
			}
			else
			{
				char* path = Shell_String_Merge_2char__char__(classStatics_Shell_Program.m_folder, command);
				Shell_Process_Run_3char__char___int32_t_(path);
			}
		}
	}
	;
}
struct class_Shell_String* classInit_Shell_String(void)
{
	struct class_Shell_String* object = calloc(1, sizeof(struct class_Shell_String));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Shell_String;
	return object;
}

int32_t Shell_String_Length_1char__(char* text)
{
	int32_t i = 0;
	for(;text[i] != '\0';i = i + 1
	)
	{
	}
	;
	return i;
}
int32_t Shell_String_IndexOf_2char__char__(char* text, char* occurence)
{
	int32_t found =  - 1;
	int32_t foundCount = 0;
	int32_t textLength = Shell_String_Length_1char__(text);
	int32_t occurenceLength = Shell_String_Length_1char__(occurence);
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
char* Shell_String_Merge_2char__char__(char* first, char* second)
{
	int32_t firstLength = Shell_String_Length_1char__(first);
	int32_t secondLength = Shell_String_Length_1char__(second);
	int32_t totalLength = firstLength + secondLength;
	char* outVal = (char*)Shell_Heap_Alloc_1int32_t_(totalLength + 1);
	for(int32_t i = 0;i < firstLength;i = i + 1
	)
	{
		outVal[i] = first[i];
	}
	;
	for(int32_t i = 0;i < secondLength;i = i + 1
	)
	{
		outVal[firstLength + i] = second[i];
	}
	;
	outVal[totalLength] = '\0';
	return Shell_Util_CharPtrToString_1char__(outVal);
}
int32_t Shell_String_Count_2char__char_(char* str, char occurence)
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
char* Shell_String_SubString_3char__int32_t_int32_t_(char* str, int32_t start, int32_t count)
{
	if(count <= 0)
		return null;
	int32_t stringLength = Shell_String_Length_1char__(str);
	if(start > stringLength)
		return "";
	char* ch = (char*)Shell_Heap_Alloc_1int32_t_(count + 1);
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
	return Shell_Util_CharPtrToString_1char__(ch);
}
int32_t Shell_String_Equals_2char__char__(char* one, char* two)
{
	int32_t oneLength = Shell_String_Length_1char__(one);
	int32_t twoLength = Shell_String_Length_1char__(two);
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
char Shell_String_ToUpper_1char_(char c)
{
	return  ( c >= 'a' && c <= 'z' )  ? (char) ( c +  ( 'A' - 'a' )  )  : c;
}
char Shell_String_ToLower_1char_(char c)
{
	if( ( c >= 65 )  &&  ( c <= 90 ) )
		c = (char) ( c + (int32_t)32 ) ;
	return c;
}
struct class_Shell_Util* classInit_Shell_Util(void)
{
	struct class_Shell_Util* object = calloc(1, sizeof(struct class_Shell_Util));
	if(!object)
		return NULL;
	object->usage_count = 1;
	object->lookup_table = methods_Shell_Util;
	return object;
}


static void* methods_Shell_Console[] = {Shell_Console_Write_1char__,Shell_Console_Write_1char_,Shell_Console_WriteLine_1char__,Shell_Console_ReadChar_0,Shell_Console_ReadLine_0,Shell_Console_WriteHex_1int64_t_,};
static void* methods_Shell_Directory[] = {Shell_Directory_OpenInternal_1char__,Shell_Directory_ReaddirInternal_3void__struct_struct_Shell_Directory_DirEntry__uint32_t_,Shell_Directory_CloseInternal_1void__,Shell_Directory_Open_1char__,Shell_Directory_Readdir_2class_uint32_t_,Shell_Directory_Close_1class_,};
static void* methods_Shell_Heap[] = {Shell_Heap_Alloc_1int32_t_,Shell_Heap_Free_1void__,};
static void* methods_Shell_Process[] = {Shell_Process_internalRun_2char__char___,Shell_Process_Run_3char__char___int32_t_,};
static void* methods_Shell_Program[] = {Shell_Program_Main_1char___,};
static void* methods_Shell_String[] = {Shell_String_Length_1char__,Shell_String_IndexOf_2char__char__,Shell_String_Merge_2char__char__,Shell_String_Count_2char__char_,Shell_String_SubString_3char__int32_t_int32_t_,Shell_String_Equals_2char__char__,Shell_String_ToUpper_1char_,Shell_String_ToLower_1char_,};
static void* methods_Shell_Util[] = {Shell_Util_CharArrayToString_1char__,Shell_Util_CharPtrToString_1char__,Shell_Util_ObjectToVoidPtr_1void__,};

void init(void)
{
	classCctor_Shell_Program();
}
