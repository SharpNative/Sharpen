global Sharpen_Arch_GDT_flushGDT_1struct_struct_Sharpen_Arch_GDT_GDT_Pointer__
Sharpen_Arch_GDT_flushGDT_1struct_struct_Sharpen_Arch_GDT_GDT_Pointer__:
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

global Sharpen_Arch_GDT_flushTSS_0
Sharpen_Arch_GDT_flushTSS_0:
    ; Index = 5
    ; Privilege = 3
    mov ax, (5 * 8) | 3 ; (Index * 8) | Privilege
    ltr ax
    ret

global Sharpen_Arch_IDT_flushIDT_1struct_struct_Sharpen_Arch_IDT_IDT_Pointer__
Sharpen_Arch_IDT_flushIDT_1struct_struct_Sharpen_Arch_IDT_IDT_Pointer__:
    ; Pointer passed as an argument
    mov eax, [esp + 4]
    lidt [eax]
    ret