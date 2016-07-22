; External functions
extern Sharpen_Arch_ISR_Handler_1struct_struct_Sharpen_Arch_Regs__
extern Sharpen_Arch_IRQ_Handler_1struct_struct_Sharpen_Arch_Regs__

; =====================================
; ===           GDT class           ===
; =====================================

global Sharpen_Arch_GDT_FlushGDT_1struct_struct_Sharpen_Arch_GDT_GDT_Pointer__
Sharpen_Arch_GDT_FlushGDT_1struct_struct_Sharpen_Arch_GDT_GDT_Pointer__:
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

global Sharpen_Arch_IDT_FlushIDT_1struct_struct_Sharpen_Arch_IDT_IDT_Pointer__
Sharpen_Arch_IDT_FlushIDT_1struct_struct_Sharpen_Arch_IDT_IDT_Pointer__:
    ; Pointer passed as an argument
    mov eax, [esp + 4]
    lidt [eax]
    ret

%macro INT_COMMON 2
    global %1
    %1:
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
        call %2
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
%endmacro

%macro ISR_NO_ERROR 1
    global Sharpen_Arch_IDT_ISR%1_0
    Sharpen_Arch_IDT_ISR%1_0:
        push 0
        push %1
        jmp isr_common
%endmacro

%macro ISR_ERROR 1
    global Sharpen_Arch_IDT_ISR%1_0
    Sharpen_Arch_IDT_ISR%1_0:
        push %1
        jmp isr_common
%endmacro

%macro IRQ 1
    global Sharpen_Arch_IDT_IRQ%1_0
    Sharpen_Arch_IDT_IRQ%1_0:
        push 0
        push (%1 + 32)
        jmp irq_common
%endmacro

; Interrupt handlers
INT_COMMON isr_common, Sharpen_Arch_ISR_Handler_1struct_struct_Sharpen_Arch_Regs__
INT_COMMON irq_common, Sharpen_Arch_IRQ_Handler_1struct_struct_Sharpen_Arch_Regs__

; ISR routines
ISR_NO_ERROR 0
ISR_NO_ERROR 1
ISR_NO_ERROR 2
ISR_NO_ERROR 3
ISR_NO_ERROR 4
ISR_NO_ERROR 5
ISR_NO_ERROR 6
ISR_NO_ERROR 7
ISR_ERROR    8
ISR_NO_ERROR 9
ISR_ERROR    10
ISR_ERROR    11
ISR_ERROR    12
ISR_ERROR    13
ISR_ERROR    14
ISR_NO_ERROR 15
ISR_NO_ERROR 16
ISR_NO_ERROR 17
ISR_NO_ERROR 18
ISR_NO_ERROR 19
ISR_NO_ERROR 20
ISR_NO_ERROR 21
ISR_NO_ERROR 22
ISR_NO_ERROR 23
ISR_NO_ERROR 24
ISR_NO_ERROR 25
ISR_NO_ERROR 26
ISR_NO_ERROR 27
ISR_NO_ERROR 28
ISR_NO_ERROR 29
ISR_NO_ERROR 30
ISR_NO_ERROR 31

; IRQ routines
IRQ 0
IRQ 1
IRQ 2
IRQ 3
IRQ 4
IRQ 5
IRQ 6
IRQ 7
IRQ 8
IRQ 9
IRQ 10
IRQ 11
IRQ 12
IRQ 13
IRQ 14
IRQ 15

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

global Sharpen_Memory_Memcpy_3void__void__int32_t_
Sharpen_Memory_Memcpy_3void__void__int32_t_:
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

global Sharpen_Memory_Memset_3void__int32_t_int32_t_
Sharpen_Memory_Memset_3void__int32_t_int32_t_:
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

global Sharpen_Util_CharPtrToString_1char__
Sharpen_Util_CharPtrToString_1char__:
    mov eax, [esp + 4]
    ret

global Sharpen_Util_ObjectToVoidPtr_1void__
Sharpen_Util_ObjectToVoidPtr_1void__:
    mov eax, [esp + 4]
    ret

global Sharpen_Util_MethodToPtr_1void__
Sharpen_Util_MethodToPtr_1void__:
    mov eax, [esp + 4]
    ret