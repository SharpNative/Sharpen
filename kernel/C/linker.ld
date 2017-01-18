OUTPUT_FORMAT(elf32-i386)
ENTRY(start)
phys = 0x00100000;

SECTIONS
{
    /* The kernel code */
    .text phys : AT(phys)
    {
        code = .;
        *(.multiboot)
        *(.text)
        *(.rodata)
        . = ALIGN(4096);
    }

    /* The kernel data */
    .data : AT(phys + (data - code))
    {
        data = .;
        *(.data)
        *(.kernelsymbols)
        PROVIDE(kernelsymbols = .);
        . = ALIGN(4096);
    }

    /* Statically defined uninitialized values */
    .bss : AT(phys + (bss - code))
    {
        bss = .;
        *(.bss)
        . = ALIGN(4096);
    }

    /* Remove unnecessary GCC stuff */
    /DISCARD/ :
    {
        *(.comment)
        *(.eh_frame)
        *(.note.gnu.build-id)
    }

    end = .;
}