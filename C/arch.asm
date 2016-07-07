; External functions
extern Sharpen_Arch_IDT_Handler_1

; =====================================
; ===           GDT class           ===
; =====================================

global Sharpen_Arch_GDT_FlushGDT_1
Sharpen_Arch_GDT_FlushGDT_1:
    ; Pointer passed as argument
    mov eax, [esp + 4]
    lgdt [eax]

    ; 0x10 is the offset in the GDT to the data segment of the kernel
    mov ax, 0x10
    mov ds, ax
    mov es, ax
    mov fs, ax
    mov gs, ax
    mov ss, ax

    ; Far jump to load the segments
    ; 0x08 is the offset in the GDT to the code segment of the kernel
    jmp 0x08:.flush
    
.flush:
    ret

; =====================================
; ===           IDT class           ===
; =====================================

global Sharpen_Arch_IDT_FlushIDT_1
Sharpen_Arch_IDT_FlushIDT_1:
    ; Pointer passed as an argument
    mov eax, [esp + 4]
    lidt [eax]
    ret

; Common interrupt handler
int_common:
    ; Store data
    pusha

    ; Store segment
    push ds
    push es
    push fs
    push gs

    ; Load kernel segment
    mov ax, 0x10
    mov ds, ax
    mov es, ax
    mov fs, ax
    mov gs, ax

    ; Call interrupt handler
    push esp
    call Sharpen_Arch_IDT_Handler_1
    add esp, 4

    ; Reload original segment
    pop gs
    pop fs
    pop es
    pop ds

    ; Pop data
    popa

    ; Cleanup (errorcode and pushed INT number)
    add esp, 8
    iret

%macro INT_NO_ERROR 1
    GLOBAL Sharpen_Arch_IDT_ISR%1_0
    Sharpen_Arch_IDT_ISR%1_0:
        push 0
        push %1
        jmp int_common
%endmacro

%macro INT_ERROR    1
    GLOBAL Sharpen_Arch_IDT_ISR%1_0
    Sharpen_Arch_IDT_ISR%1_0:
        push %1
        jmp int_common
%endmacro

; ISR routines
INT_NO_ERROR 0
INT_NO_ERROR 1
INT_NO_ERROR 2
INT_NO_ERROR 3
INT_NO_ERROR 4
INT_NO_ERROR 5
INT_NO_ERROR 6
INT_NO_ERROR 7
INT_ERROR    8
INT_NO_ERROR 9
INT_ERROR    10
INT_ERROR    11
INT_ERROR    12
INT_ERROR    13
INT_ERROR    14
INT_NO_ERROR 15
INT_NO_ERROR 16
INT_NO_ERROR 17
INT_NO_ERROR 18
INT_NO_ERROR 19
INT_NO_ERROR 20
INT_NO_ERROR 21
INT_NO_ERROR 22
INT_NO_ERROR 23
INT_NO_ERROR 24
INT_NO_ERROR 25
INT_NO_ERROR 26
INT_NO_ERROR 27
INT_NO_ERROR 28
INT_NO_ERROR 29
INT_NO_ERROR 30
INT_NO_ERROR 31

global Sharpen_Arch_IDT_INTIgnore_0
Sharpen_Arch_IDT_INTIgnore_0:
    ; Ignored interrupt
    iret

; =====================================
; ===           CPU class           ===
; =====================================

global Sharpen_Arch_CPU_CLI_0
Sharpen_Arch_CPU_CLI_0:
    cli
    ret

global Sharpen_Arch_CPU_STI_0
Sharpen_Arch_CPU_STI_0:
    sti
    ret

global Sharpen_Arch_CPU_HLT_0
Sharpen_Arch_CPU_HLT_0:
    hlt
    ret

; =====================================
; ===         Memory class          ===
; =====================================

global Sharpen_Memory_Memcpy_3
Sharpen_Memory_Memcpy_3:
    push edi
    push esi

    ; First copy per 4 bytes using movsd
    ; Then copy the remaining bytes
    ; Note: count >> 2 = count / 4
    ; Note: count & 3 = count % 4

    mov eax, [esp + 20] ; Count
    mov esi, [esp + 16] ; Source
    mov edi, [esp + 12] ; Destination

    mov ecx, eax

    ; Remaining part
    sar ecx, 2
    and eax, 3

    ; Clear direction flag
    cld

    ; First part (as 32bit ints)
    rep movsd

    ; Second part (as bytes)
    mov eax, ecx
    rep movsb

    ; Done
    mov eax, edi
    pop esi
    pop edi
    ret

global Sharpen_Memory_Memset_3
Sharpen_Memory_Memset_3:
    push edi

    ; First set per 4 bytes using stosd
    ; Then set the remaining bytes
    ; Note: count >> 2 = count / 4
    ; Note: count & 3 = count % 4

    mov eax, [esp + 16] ; Count
    mov edi, [esp + 8] ; Destination

    mov ecx, eax
    mov edx, eax

    ; Remaining part
    sar ecx, 2
    and edx, 3

    ; Load value
    mov eax, [esp + 12]

    ; Clear direction flag
    cld

    ; First part (as 32bit ints)
    rep stosd

    ; Second part (as bytes)
    mov edx, ecx
    rep stosb

    ; Done
    mov eax, edi
    pop edi
    ret

; =====================================
; ===          Util class           ===
; =====================================

global Sharpen_Util_CharPtrToString_1
Sharpen_Util_CharPtrToString_1:
    mov eax, [esp + 4]
    ret

global Sharpen_Util_MethodToPtr_1
Sharpen_Util_MethodToPtr_1:
    mov eax, [esp + 4]
    ret