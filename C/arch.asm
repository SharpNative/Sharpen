global Sharpen_Arch_GDT_FlushGDT
Sharpen_Arch_GDT_FlushGDT:
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