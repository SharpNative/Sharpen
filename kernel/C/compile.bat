@echo off
call ..\..\scripts\pre

echo Assembling..
%AS% %KERNEL_ASM_FLAGS% arch\arch.asm -o build\arch.o
%AS% %KERNEL_ASM_FLAGS% arch\int19.asm -o build\int19.o

echo Compiling..
%CC% %KERNEL_C_FLAGS% kernel.c -c -o build\kernel.o

echo Generating symbols...
%LD% %KERNEL_LD_FLAGS% build\*.o -o kernel
%NM% kernel -g > symbols.txt
%DUMP_SYMBOLS% symbols.txt symbols.asm
%AS% %KERNEL_ASM_FLAGS% symbols.asm -o build\symbols.o

echo Linking kernel...
%LD% %KERNEL_LD_FLAGS% -s build\*.o -o kernel

echo Copying kernel...
call ..\..\scripts\mount
copy kernel G:\kernel
call ..\..\scripts\unmount

call ..\..\scripts\qemu.bat