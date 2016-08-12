OUTPUT_FORMAT(elf32-i386)
ENTRY(_start)
STARTUP(newlib/crt0.o)
phys = 0x8048000;

SECTIONS
{
    .text phys : AT(phys)
    {
        code = .;
        *(.text)
        *(.rodata)
    }

    .data : AT(phys + (data - code))
    {
        data = .;
        *(.data)
    }

    .bss : AT(phys + (bss - code))
    {
        bss = .;
        *(.bss)
    }
}