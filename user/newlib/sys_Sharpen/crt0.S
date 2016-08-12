.extern main
.global _start

# Global entry point of application
_start:
    # C main function
    call main

    # The return code from the main method is in EAX register
    # The EAX register for the syscall is the syscall number
    # So transfer the contents of EAX to the parameter EBX
    mov %ebx, %eax
    mov $0, %eax
    int $0x80

.loop:
    hlt
    jmp .loop