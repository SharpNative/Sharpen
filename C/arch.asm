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