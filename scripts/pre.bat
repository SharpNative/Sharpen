set PATH=%PATH%;..\..\tools\gcc\bin;..\..\tools\nasm

if exist build goto nocreatedir
mkdir build

:nocreatedir
del build\*.o 2>NUL

rem Tools
set AS=nasm
set CC=i686-sharpen-gcc
set NM=i686-sharpen-nm
set LD=i686-sharpen-ld

rem Userspace
set USER_C_FLAGS=-Wall -s -O1 -std=c99

rem Kernelspace
set KERNEL_ASM_FLAGS=-f elf32
set KERNEL_C_FLAGS=-Wall -O1 -fno-omit-frame-pointer -std=c99
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