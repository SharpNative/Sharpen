global Sharpen_Synchronisation_Mutex_InternalLock_1int32_t__
Sharpen_Synchronisation_Mutex_InternalLock_1int32_t__:
    ; Argument is a pointer
    mov eax, [esp + 4]

    ; Test and set
    mov edx, 1
.tryAcquiring:
    ; The XCHG instruction implies a LOCK, an explicit LOCK prefix does nothing but wasting space
    xchg edx, [eax]
    test edx, edx
    je .acquired

    ; Switch because we're waiting
    int 0x81 ; Yield

    jmp .tryAcquiring
.acquired:
    ret

global Sharpen_Synchronisation_Mutex_InternalUnlock_1int32_t__
Sharpen_Synchronisation_Mutex_InternalUnlock_1int32_t__:
    ; Argument is a pointer
    mov eax, [esp + 4]
    mov [eax], dword 0
    ret

global Sharpen_Synchronisation_Spinlock_InternalLock_1int32_t__
Sharpen_Synchronisation_Spinlock_InternalLock_1int32_t__:
    ; Argument is a pointer
    mov eax, [esp + 4]

    ; Test and set
    mov edx, 1
.tryAcquiring:
    ; The XCHG instruction implies a LOCK, an explicit LOCK prefix does nothing but wasting space
    xchg edx, [eax]
    test edx, edx
    je .acquired
    jmp .tryAcquiring
.acquired:
    ret

global Sharpen_Synchronisation_Spinlock_InternalUnlock_1int32_t__
Sharpen_Synchronisation_Spinlock_InternalUnlock_1int32_t__:
    ; Argument is a pointer
    mov eax, [esp + 4]
    mov [eax], dword 0
    ret