@echo off
del build\*.o
set PATH=%PATH%;tools\gcc\bin
echo Assembling..
"tools\Nasm\nasm.exe" -f elf32 arch\arch.asm -o build\arch.o
echo Compiling..
"tools\gcc\bin\i686-elf-gcc.exe" -I./include -m32 -Wall -O2 -std=c99 -nostdlib kernel.c -c -o build\kernel.o
echo Linking kernel...
"tools\gcc\bin\i686-elf-ld.exe" -Tlinker.ld -s build\*.o -o kernel
echo Copying kernel...
imdisk -a -m G: -o hd -t file -f os.img -v 1
copy kernel G:\kernel
imdisk -D -m G:
echo Starting QEMU...
"C:\Program Files\qemu\qemu-system-i386.exe" os.img -m 256