; Task scheduler
extern Sharpen_Task_Tasking_scheduler_1struct_struct_Sharpen_Arch_Regs__
; PIT handler
extern Sharpen_Arch_PIT_Handler_1struct_struct_Sharpen_Arch_Regs__
; Syscall handler
extern Sharpen_Arch_Syscall_Handler_1struct_struct_Sharpen_Arch_Regs__

%macro INT_COMMON 2
    extern %2
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

; Syscall handler
global Sharpen_Arch_IDT_Syscall_0
Sharpen_Arch_IDT_Syscall_0:
    ; No errorcode or INT number
    sub esp, 8

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
    call Sharpen_Arch_Syscall_Handler_1struct_struct_Sharpen_Arch_Regs__
    add esp, 4

    ; Reload original segment
    pop gs
    pop fs
    pop es
    pop ds

    ; Pop data
    popa
    add esp, 8
    iret

; Special IRQ routine for IRQ 0 (PIT)
global Sharpen_Arch_IDT_IRQ0_0
Sharpen_Arch_IDT_IRQ0_0:
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

    ; Call both the PIT handler and the task scheduler
    push esp
    call Sharpen_Arch_PIT_Handler_1struct_struct_Sharpen_Arch_Regs__
    call Sharpen_Task_Tasking_scheduler_1struct_struct_Sharpen_Arch_Regs__
    mov esp, eax

    ; Acknowledge IRQ
    mov al, 0x20
    out 0x20, al

    ; Reload original segment
    pop gs
    pop fs
    pop es
    pop ds

    ; Pop data
    popa
    iret

; Manual scheduler
global Sharpen_Task_Tasking_ManualSchedule_0
Sharpen_Task_Tasking_ManualSchedule_0:
    ; Setup interrupt stack frame
    pushfd
    push cs
    push .r

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

    ; Task scheduler
    push esp
    call Sharpen_Task_Tasking_scheduler_1struct_struct_Sharpen_Arch_Regs__
    mov esp, eax

    ; Reload original segment
    pop gs
    pop fs
    pop es
    pop ds

    ; Pop data
    popa
    iret
.r:
    ret

; IRQ routines
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