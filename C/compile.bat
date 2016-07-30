@echo off
del build\*.o
echo Assembling..
"tools\Nasm\nasm.exe" -f elf32 load.asm -o build\load.o
"tools\Nasm\nasm.exe" -f elf32 portio.asm -o build\portio.o
"tools\Nasm\nasm.exe" -f elf32 arch.asm -o build\arch.o
echo Compiling..
"tools\gcc\bin\i586-elf-gcc.exe" -I./include -m32 -Wall -O2 -std=c99 -nostdlib kernel.c -c -o build\kernel.o
echo Linking kernel...
"tools\gcc\bin\i586-elf-ld.exe" -Tlinker.ld -s build\*.o -o kernel
copy kernel G:\kernel > NUL
pause;