call ..\..\scripts\pre

echo Compiling...
%CC% %USER_C_FLAGS% ../SharpenLib/SharpenLib.c %1/C/output.c -o %1.elf

echo Copying...
call ..\..\scripts\mount
copy %1.elf G:\exec\%1
call ..\..\scripts\unmount

call ..\..\scripts\qemu