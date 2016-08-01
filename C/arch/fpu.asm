global Sharpen_Arch_FPU_Init_0
Sharpen_Arch_FPU_Init_0:
    ; Set OSFXSR bit (bit 9) in CR4
    mov eax, cr4
    or eax, (1 << 9)
    mov cr4, eax

    ; Clear the EM bit (bit 2), set NE bit (bit 5) and set MP bit (bit 1) in CR0
    mov eax, cr0
    and eax, ~(1 << 2)
    or eax, (1 << 5)
    or eax, (1 << 1)
    mov cr0, eax

    ; Default configuration
    fninit

    ret

global Sharpen_Arch_FPU_RestoreContext_1void__
Sharpen_Arch_FPU_RestoreContext_1void__:
    mov eax, [esp + 4]
    fxrstor [eax]
    ret

global Sharpen_Arch_FPU_StoreContext_1void__
Sharpen_Arch_FPU_StoreContext_1void__:
    mov eax, [esp + 4]
    fxsave [eax]
    ret