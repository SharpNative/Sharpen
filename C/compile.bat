@echo off
set PATH=%PATH%;..\tools\gcc\bin

IF EXIST build GOTO nocreatedir
mkdir build

:nocreatedir
del build\*.o

echo Assembling..
"../tools/nasm/nasm" -f elf32 arch\arch.asm -o build\arch.o

echo Compiling..
i686-elf-gcc -I./include -m32 -Wall -O2 -std=c99 -nostdlib kernel.c -c -o build\kernel.o

echo Linking kernel...
i686-elf-ld -Tlinker.ld -s build\*.o -o kernel

echo Copying kernel...
imdisk -a -m G: -o hd -t file -f os.img -v 1
copy kernel G:\kernel
imdisk -D -m G:

echo Starting QEMU...
"C:\Program Files\qemu\qemu-system-i386.exe" os.img -soundhw ac97 -net nic,model=rtl8139 -net dump,file=netdump.wcap -net user -m 256 -D qemu.log