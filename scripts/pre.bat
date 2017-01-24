set PATH=%PATH%;..\..\tools\gcc\bin;..\..\tools\nasm

if exist build goto nocreatedir
mkdir build

:nocreatedir
del build\*.o 2>NUL

rem Userspace
set USER_C_FLAGS=-I../newlib/include -Wall -O2 -std=c99
set USER_LD_FLAGS=-s -T../link.ld

rem Kernelspace
set KERNEL_ASM_FLAGS=-f elf32
set KERNEL_C_FLAGS=-I./include -Wall -O0 -fno-omit-frame-pointer -std=c99
set KERNEL_LD_FLAGS=-Tlinker.ld

rem Utilities
set DUMP_SYMBOLS=..\..\Util\DumpSymbols\bin\Release\DumpSymbols.exe

rem Disk image
set DRIVE_LETTER=G

rem Checks
if not exist %DUMP_SYMBOLS% (
	echo Compile the Util solution first before you want to compile anything
	pause
	exit
)