section .usercode

global Sharpen_Exec_SignalAction_ReturnFromSignal_0
Sharpen_Exec_SignalAction_ReturnFromSignal_0:
    ; Restore the previous syscall registers
    mov eax, 22
    int 0x80

    ; It is impossible to get here because the syscall overwrites the return address
    jmp 0xDEADBEEF