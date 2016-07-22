BITS 32
ALIGN 4

global start
extern init
extern end
extern Sharpen_Program_KernelMain_3struct_struct_Sharpen_Multiboot_Header__uint32_t_uint32_t_

SECTION .multiboot
mboot:
    MULTIBOOT_PAGE_ALIGN    equ 1 << 0
    MULTIBOOT_MEMORY_INFO   equ 1 << 1
    MULTIBOOT_HEADER_MAGIC  equ 0x1BADB002
    MULTIBOOT_HEADER_FLAGS  equ MULTIBOOT_PAGE_ALIGN | MULTIBOOT_MEMORY_INFO
    MULTIBOOT_CHECKSUM      equ -(MULTIBOOT_HEADER_MAGIC + MULTIBOOT_HEADER_FLAGS)
    
    dd MULTIBOOT_HEADER_MAGIC
    dd MULTIBOOT_HEADER_FLAGS
    dd MULTIBOOT_CHECKSUM
    
    ; AOUT locations, but we use ELF so we don't need to use this
    dd 0
    dd 0
    dd 0
    dd 0
    dd 0

SECTION .text
start:
    ; Set stack
    mov esp, _sys_stack

    ; Push data now for the kernel main
    push dword end  ; Linker end
    push eax        ; Magic
    push ebx        ; Multiboot header

    ; Call .cctors
    call init
    ; Go to kernel main
    call Sharpen_Program_KernelMain_3struct_struct_Sharpen_Multiboot_Header__uint32_t_uint32_t_

    ; Gets here if unexpected end
    cli

.halt:
    hlt
    jmp .halt

SECTION .bss
    resb 8192

_sys_stack: