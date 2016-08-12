@echo off
set PATH=%PATH%;..\tools\gcc\bin

IF EXIST build GOTO nocreatedir
mkdir build

:nocreatedir
del build\*.o

echo Compiling...
i686-elf-gcc -I./newlib/include -m32 -Wall -O2 -std=c99 -fno-builtin test.c -c -o build\test.o

echo Linking...
i686-elf-ld -s -Tlink.ld build\test.o newlib\libc.a -o test

pause