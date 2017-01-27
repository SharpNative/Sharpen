call ..\..\scripts\pre

echo Compiling...
%CC% %USER_C_FLAGS% %1.c -o %1.elf

echo Copying...
call ..\..\scripts\mount
copy %1.elf G:\exec\%1
call ..\..\scripts\unmount

call ..\..\scripts\qemu