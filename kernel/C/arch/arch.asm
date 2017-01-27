section .text

%include "arch/cpu.asm"
%include "arch/fpu.asm"
%include "arch/descriptors.asm"
%include "arch/interrupts.asm"
%include "arch/portio.asm"
%include "arch/memory.asm"
%include "arch/paging.asm"
%include "arch/mutex.asm"
%include "arch/usercode.asm"
%include "arch/load.asm"