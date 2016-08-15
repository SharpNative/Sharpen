@echo off
set PATH=%PATH%;..\..\..\tools\gcc\bin

IF EXIST build GOTO nocreatedir
mkdir build

:nocreatedir
del build\*.o

echo Compiling...
i686-elf-gcc -I../../newlib/include -Wall -O2 -std=c99 test.c -c -o build\test.o

echo Linking...
i686-elf-ld -s -T..\..\link.ld build\test.o ..\..\newlib\libc.a -o test

echo Copying...
imdisk -a -m G: -o hd -t file -f ..\..\..\kernel\C\os.img -v 1
copy test G:\test
imdisk -D -m G:

echo Starting QEMU...
"C:\Program Files\qemu\qemu-system-i386.exe" ..\..\..\kernel\C\os.img -soundhw ac97 -net nic,model=rtl8139 -net dump,file=netdump.wcap -net user -m 256 -D qemu.log