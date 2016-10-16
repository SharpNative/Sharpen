call ..\..\scripts\pre

set FILE=%1

echo Compiling...
i686-elf-gcc %USER_C_FLAGS% %FILE%.c -c -o build\%FILE%.o

echo Linking...
i686-elf-ld %USER_LD_FLAGS% build\%FILE%.o ..\newlib\libc.a -o %FILE%.elf

echo Copying...
call ..\..\scripts\mount
copy %FILE%.elf G:\%FILE%
call ..\..\scripts\unmount
rem del %FILE%.elf

call ..\..\scripts\qemu