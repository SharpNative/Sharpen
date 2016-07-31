global Sharpen_Arch_Paging_Enable_0
Sharpen_Arch_Paging_Enable_0:
    mov eax, cr0
    or eax, 0x80000000
    mov cr0, eax
    ret

global Sharpen_Arch_Paging_Disable_0
Sharpen_Arch_Paging_Disable_0:
    mov eax, cr0
    and eax, 0x7FFFFFFF
    mov cr0, eax
    ret

global Sharpen_Arch_Paging_SetDirectoryInternal_1struct_struct_Sharpen_Arch_Paging_PageDirectory__
Sharpen_Arch_Paging_SetDirectoryInternal_1struct_struct_Sharpen_Arch_Paging_PageDirectory__:
    mov eax, [esp + 4]
    mov cr3, eax
    ret