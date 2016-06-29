@echo off
del build\*.o
"tools\Nasm\nasm.exe" -f elf32 load.asm -o build\load.o
"tools\Nasm\nasm.exe" -f elf32 portio.asm -o build\portio.o
"tools\gcc\bin\i586-elf-gcc.exe" -I./include -m32 -Wall -O2 -std=c99 -nostdlib kernel.c -c -o build\kernel.o
"tools\gcc\bin\i586-elf-gcc.exe" -I./include -m32 -Wall -O2 -std=c99 -nostdlib heap.c -c -o build\heap.o
"tools\gcc\bin\i586-elf-ld.exe" -Tlinker.ld -s build\*.o -o kernel
copy kernel F:\kernel > NUL