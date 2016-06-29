/* Method prototypes */
void Sharpen_Console_PutChar(char ch);
void Sharpen_Console_Write(char* text);
void Sharpen_Console_WriteLine(char* text);
void Sharpen_Program_KernelMain(void);
int32_t Sharpen_String_Length(char* text);
/* Enums */

/* Namespace <Sharpen> */
/* Class <Console> */
struct class_Sharpen_Console
{
	int32_t usage_count;
};

struct
{
	/* Static Field: vidmem */
	uint8_t* vidmem;
	/* Static Property: X */
	int32_t prop_X;
	/* Static Property: Y */
	int32_t prop_Y;
} classStatics_Sharpen_Console;

void classCctor_Sharpen_Console(void)
{
	classStatics_Sharpen_Console.vidmem = (uint8_t*)0xB8000;
	classStatics_Sharpen_Console.prop_X = 0;
	classStatics_Sharpen_Console.prop_Y = 0;
}

struct class_Sharpen_Console* classInit_Sharpen_Console(void)
{
	struct class_Sharpen_Console* object = malloc(sizeof(struct class_Sharpen_Console));
	if(!object)
		return NULL;
	object->usage_count = 1;
	return object;
}

int32_t Sharpen_Console_X_getter(struct class_Sharpen_Console* obj)
{
	return classStatics_Sharpen_Console.prop_X;
}
int32_t Sharpen_Console_X_setter(struct class_Sharpen_Console* obj, int32_t value)
{
	classStatics_Sharpen_Console.prop_X = value;
	return value;
}
int32_t Sharpen_Console_Y_getter(struct class_Sharpen_Console* obj)
{
	return classStatics_Sharpen_Console.prop_Y;
}
int32_t Sharpen_Console_Y_setter(struct class_Sharpen_Console* obj, int32_t value)
{
	classStatics_Sharpen_Console.prop_Y = value;
	return value;
}
/* Static Method <PutChar> */
void Sharpen_Console_PutChar(char ch)
{
	if(ch == '\n')
	{
		Sharpen_Console_X_setter(NULL, 0);
		Sharpen_Console_Y_setter(NULL, Sharpen_Console_Y_getter(NULL) + 1);
	}
	else
	{
		classStatics_Sharpen_Console.vidmem[ ( Sharpen_Console_Y_getter(NULL) * 25 + Sharpen_Console_X_getter(NULL) )  * 2 + 0] = (uint8_t)ch;
		classStatics_Sharpen_Console.vidmem[ ( Sharpen_Console_Y_getter(NULL) * 25 + Sharpen_Console_X_getter(NULL) )  * 2 + 1] = 0x07;
		Sharpen_Console_X_setter(NULL, Sharpen_Console_X_getter(NULL) + 1);
	}
	if(Sharpen_Console_X_getter(NULL) == 80)
	{
		Sharpen_Console_X_setter(NULL, 0);
		Sharpen_Console_Y_setter(NULL, Sharpen_Console_Y_getter(NULL) + 1);
	}
}
/* Static Method <Write> */
void Sharpen_Console_Write(char* text)
{
	for(/* Variable i = 0 */
	int32_t i = 0;
	i < Sharpen_String_Length(text);
	i = i + 1	)
	{
	}
}
/* Static Method <WriteLine> */
void Sharpen_Console_WriteLine(char* text)
{
	Sharpen_Console_Write(text);
	Sharpen_Console_PutChar('\n');
}
/* Namespace <Sharpen> */
/* Class <Program> */
struct class_Sharpen_Program
{
	int32_t usage_count;
};

struct
{
} classStatics_Sharpen_Program;

void classCctor_Sharpen_Program(void)
{
}

struct class_Sharpen_Program* classInit_Sharpen_Program(void)
{
	struct class_Sharpen_Program* object = malloc(sizeof(struct class_Sharpen_Program));
	if(!object)
		return NULL;
	object->usage_count = 1;
	return object;
}

/* Static Method <KernelMain> */
void Sharpen_Program_KernelMain(void)
{
	Sharpen_Console_WriteLine("test test");
}
/* Namespace <Sharpen> */
/* Class <String> */
struct class_Sharpen_String
{
	int32_t usage_count;
};

struct
{
} classStatics_Sharpen_String;

void classCctor_Sharpen_String(void)
{
}

struct class_Sharpen_String* classInit_Sharpen_String(void)
{
	struct class_Sharpen_String* object = malloc(sizeof(struct class_Sharpen_String));
	if(!object)
		return NULL;
	object->usage_count = 1;
	return object;
}

/* Static Method <Length> */
int32_t Sharpen_String_Length(char* text)
{
	/* Variable i = 0 */
	int32_t i = 0;
	for(;text[i] != '\0'i = i + 1	)
	{
	}
	return i;
}
void init(void)
{
	classCctor_Sharpen_Console();
	classCctor_Sharpen_Program();
	classCctor_Sharpen_String();
}
