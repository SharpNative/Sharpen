global Sharpen_Mutex_InternalLock_1int32_t__
Sharpen_Mutex_InternalLock_1int32_t__:
    ; Argument is a pointer
    mov eax, [esp + 4]

    push ecx
    
    ; Test and set
    mov ecx, 1
.tryAcquiring:
    ; The XCHG instruction implies a LOCK, an explicit LOCK prefix does nothing but wasting space
    xchg ecx, [eax]
    test ecx, ecx
    je .acquired

    ; Switch because we're waiting
    call Sharpen_Task_Tasking_ManualSchedule_0

    jmp .tryAcquiring
.acquired:
    pop ecx
    ret

global Sharpen_Mutex_InternalUnlock_1int32_t__
Sharpen_Mutex_InternalUnlock_1int32_t__:
    ; Argument is a pointer
    mov eax, [esp + 4]
    mov [eax], dword 0
    ret