.global _setjmp
.global setjmp
setjmp:
_setjmp:
    # jmp_buf is here
    movl 4(%esp), %ecx

    # When a call is made, the return address is pushed on the stack
    # Grab the return address so we know the EIP
    popl %edx

    # Registers
    movl %ebx, 0(%ecx)
    movl %esi, 4(%ecx)
    movl %edi, 8(%ecx)
    
    # Frame pointer, stack pointer, eip
    movl %ebp, 12(%ecx)
    movl %esp, 16(%ecx)
    movl %edx, 20(%ecx)

    # Return zero
    # Note: we do a regular return because we popped the return address
    #       so we need to jump to the return address
    xorl %eax, %eax
    jmp *%edx

.global _longjmp
.global longjmp
longjmp:
_longjmp:
    # Return value
    movl 8(%esp), %eax

    # jmp_buf is here
    movl 4(%esp), %ecx

    # Restore registers
    movl 0(%ecx), %ebx
    movl 4(%ecx), %esi
    movl 8(%ecx), %edi

    # Restore framepointer and stack pointer
    movl 12(%ecx), %ebp
    movl 16(%ecx), %esp

    # Return value
    testl %eax, %eax
    jne .a

    incl %eax
.a:
    jmp *20(%ecx)