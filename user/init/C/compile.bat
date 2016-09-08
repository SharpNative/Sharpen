@echo off
set PATH=%PATH%;..\..\..\tools\gcc\bin

IF EXIST build GOTO nocreatedir
mkdir build

:nocreatedir
del build\*.o

echo Compiling...
i686-elf-gcc -I../../newlib/include -Wall -O2 -std=c99 init.c -c -o build\init.o

echo Linking...
i686-elf-ld -s -T..\..\link.ld build\init.o ..\..\newlib\libc.a -o init

echo Copying...
imdisk -a -m G: -o hd -t file -f ..\..\..\kernel\C\os.img -v 1
copy init G:\init
imdisk -D -m G:
del init

echo Starting QEMU...
"C:\Program Files\qemu\qemu-system-i386.exe" ..\..\..\kernel\C\os.img -soundhw ac97 -net nic,model=rtl8139 -net dump,file=netdump.wcap -net user -m 256 -D qemu.log