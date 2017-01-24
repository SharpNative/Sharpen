BITS 32
ALIGN 4

global start
extern init
extern end
extern Sharpen_Program_KernelMain_3struct_struct_Sharpen_Multiboot_Header__uint32_t_uint32_t_

section .multiboot
mboot:
    MULTIBOOT_PAGE_ALIGN    equ 1 << 0
    MULTIBOOT_MEMORY_INFO   equ 1 << 1
    MULTIBOOT_HEADER_MAGIC  equ 0x1BADB002
    MULTIBOOT_HEADER_FLAGS  equ MULTIBOOT_PAGE_ALIGN | MULTIBOOT_MEMORY_INFO
    MULTIBOOT_CHECKSUM      equ -(MULTIBOOT_HEADER_MAGIC + MULTIBOOT_HEADER_FLAGS)
    
    dd MULTIBOOT_HEADER_MAGIC
    dd MULTIBOOT_HEADER_FLAGS
    dd MULTIBOOT_CHECKSUM

section .text
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

    ; Should actually never be able to get here
    ; this is here, just in case...
    cli
.halt:
    hlt
    jmp .halt

section .bss
resb 16384
_sys_stack: