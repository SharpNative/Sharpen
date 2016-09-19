set PATH=%PATH%;..\..\tools\gcc\bin;..\..\tools\nasm

IF EXIST build GOTO nocreatedir
mkdir build

:nocreatedir
del build\*.o

rem USERSPACE
set USER_C_FLAGS=-I../newlib/include -Wall -O2 -std=c99
set USER_LD_FLAGS=-s -T../link.ld

rem KERNELSPACE
set KERNEL_ASM_FLAGS=-f elf32
set KERNEL_C_FLAGS=-I./include -Wall -O2 -std=c99
set KERNEL_LD_FLAGS=-Tlinker.ld -s

rem DISK IMAGE
set DRIVE_LETTER=G