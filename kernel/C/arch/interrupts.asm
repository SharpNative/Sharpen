extern Sharpen_MultiTasking_Tasking_Scheduler_1void__
extern Sharpen_Arch_Syscall_Handler_1struct_struct_Sharpen_Arch_RegsDirect__
extern Sharpen_Arch_LocalApic_timerHandler_1void__

%macro INT_COMMON 2
    extern %2
    global %1
    %1:
        ; Store data
        pusha

        ; Store segment
        push ds
        push es
        push fs
        push gs

        ; Load kernel segment
        mov ax, 0x10
        mov ds, ax
        mov es, ax
        mov fs, ax
        mov gs, ax

        ; Call interrupt handler
        push esp
        call %2
        mov esp, eax

        ; Reload original segment
        pop gs
        pop fs
        pop es
        pop ds

        ; Pop data
        popa

        ; Cleanup (errorcode and pushed INT number)
        add esp, 8
        iret
%endmacro

%macro ISR_NO_ERROR 1
    global Sharpen_Arch_IDT_ISR%1_0
    Sharpen_Arch_IDT_ISR%1_0:
        push 0
        push %1
        jmp isr_common
%endmacro

%macro ISR_ERROR 1
    global Sharpen_Arch_IDT_ISR%1_0
    Sharpen_Arch_IDT_ISR%1_0:
        push %1
        jmp isr_common
%endmacro

%macro REQUEST 1
    global Sharpen_Arch_IDT_Request%1_0
    Sharpen_Arch_IDT_Request%1_0:
        push 0
        push %1
        jmp irq_common
%endmacro

; Interrupt handlers
INT_COMMON isr_common, Sharpen_Arch_ISR_Handler_1struct_struct_Sharpen_Arch_Regs__
INT_COMMON irq_common, Sharpen_Arch_IRQ_Handler_1struct_struct_Sharpen_Arch_Regs__

; ISR routines
ISR_NO_ERROR 0
ISR_NO_ERROR 1
ISR_NO_ERROR 2
ISR_NO_ERROR 3
ISR_NO_ERROR 4
ISR_NO_ERROR 5
ISR_NO_ERROR 6
ISR_NO_ERROR 7
ISR_ERROR    8
ISR_NO_ERROR 9
ISR_ERROR    10
ISR_ERROR    11
ISR_ERROR    12
ISR_ERROR    13
ISR_ERROR    14
ISR_NO_ERROR 15
ISR_NO_ERROR 16
ISR_NO_ERROR 17
ISR_NO_ERROR 18
ISR_NO_ERROR 19
ISR_NO_ERROR 20
ISR_NO_ERROR 21
ISR_NO_ERROR 22
ISR_NO_ERROR 23
ISR_NO_ERROR 24
ISR_NO_ERROR 25
ISR_NO_ERROR 26
ISR_NO_ERROR 27
ISR_NO_ERROR 28
ISR_NO_ERROR 29
ISR_NO_ERROR 30
ISR_NO_ERROR 31

; Timer handler
global Sharpen_Arch_IDT_Request0_0
Sharpen_Arch_IDT_Request0_0:
    cli
    ; Store data
    pusha

    ; Store segment
    push ds
    push es
    push fs
    push gs

    ; Load kernel segment
    mov ax, 0x10
    mov ds, ax
    mov es, ax
    mov fs, ax
    mov gs, ax

    ; Call interrupt handler
    push esp
    call Sharpen_Arch_LocalApic_timerHandler_1void__
    mov esp, eax

    ; Reload original segment
    pop gs
    pop fs
    pop es
    pop ds

    ; Pop data
    popa
    iret

; Syscall handler
global Sharpen_Arch_IDT_Syscall_0
Sharpen_Arch_IDT_Syscall_0:
    ; Store data
    pusha

    ; Store segment
    push ds
    push es
    push fs
    push gs

    ; Load kernel segment
    mov ax, 0x10
    mov ds, ax
    mov es, ax
    mov fs, ax
    mov gs, ax

    ; Call interrupt handler
    push esp
    call Sharpen_Arch_Syscall_Handler_1struct_struct_Sharpen_Arch_RegsDirect__
    add esp, 4

    ; Reload original segment
    pop gs
    pop fs
    pop es
    pop ds

    ; Pop data
    popa
    iret

; Special INT routine for INT 0x81 (Yield)
global Sharpen_Arch_IDT_Yield_0
Sharpen_Arch_IDT_Yield_0:
    ; Store data
    pusha

    ; Store segment
    push ds
    push es
    push fs
    push gs

    ; Load kernel segment
    mov ax, 0x10
    mov ds, ax
    mov es, ax
    mov fs, ax
    mov gs, ax

    ; Call the task scheduler
    push esp
    call Sharpen_MultiTasking_Tasking_Scheduler_1void__
    mov esp, eax

    ; Reload original segment
    pop gs
    pop fs
    pop es
    pop ds

    ; Pop data
    popa
    iret

; Manual scheduler
global Sharpen_MultiTasking_Tasking_Yield_0
Sharpen_MultiTasking_Tasking_Yield_0:
    sti
    int 0x81
    ret

; Requests
REQUEST 1
REQUEST 2
REQUEST 3
REQUEST 4
REQUEST 5
REQUEST 6
REQUEST 7
REQUEST 8
REQUEST 9
REQUEST 10
REQUEST 11
REQUEST 12
REQUEST 13
REQUEST 14
REQUEST 15
REQUEST 16
REQUEST 17
REQUEST 18
REQUEST 19
REQUEST 20
REQUEST 21
REQUEST 22
REQUEST 23
REQUEST 24
REQUEST 25
REQUEST 26
REQUEST 27
REQUEST 28
REQUEST 29
REQUEST 30
REQUEST 31
REQUEST 32
REQUEST 33
REQUEST 34
REQUEST 35
REQUEST 36
REQUEST 37
REQUEST 38
REQUEST 39
REQUEST 40
REQUEST 41
REQUEST 42
REQUEST 43
REQUEST 44
REQUEST 45
REQUEST 46
REQUEST 47
REQUEST 48
REQUEST 49
REQUEST 50
REQUEST 51
REQUEST 52
REQUEST 53
REQUEST 54
REQUEST 55
REQUEST 56
REQUEST 57
REQUEST 58
REQUEST 59
REQUEST 60
REQUEST 61
REQUEST 62
REQUEST 63
REQUEST 64
REQUEST 65
REQUEST 66
REQUEST 67
REQUEST 68
REQUEST 69
REQUEST 70
REQUEST 71
REQUEST 72
REQUEST 73
REQUEST 74
REQUEST 75
REQUEST 76
REQUEST 77
REQUEST 78
REQUEST 79
REQUEST 80
REQUEST 81
REQUEST 82
REQUEST 83
REQUEST 84
REQUEST 85
REQUEST 86
REQUEST 87
REQUEST 88
REQUEST 89
REQUEST 90
REQUEST 91
REQUEST 92
REQUEST 93
REQUEST 94
REQUEST 97
REQUEST 98
REQUEST 99
REQUEST 100
REQUEST 101
REQUEST 102
REQUEST 103
REQUEST 104
REQUEST 105
REQUEST 106
REQUEST 107
REQUEST 108
REQUEST 109
REQUEST 110
REQUEST 111
REQUEST 112
REQUEST 113
REQUEST 114
REQUEST 115
REQUEST 116
REQUEST 117
REQUEST 118
REQUEST 119
REQUEST 120
REQUEST 121
REQUEST 122
REQUEST 123
REQUEST 124
REQUEST 125
REQUEST 126
REQUEST 127
REQUEST 128
REQUEST 129
REQUEST 130
REQUEST 131
REQUEST 132
REQUEST 133
REQUEST 134
REQUEST 135
REQUEST 136
REQUEST 137
REQUEST 138
REQUEST 139
REQUEST 140
REQUEST 141
REQUEST 142
REQUEST 143
REQUEST 144
REQUEST 145
REQUEST 146
REQUEST 147
REQUEST 148
REQUEST 149
REQUEST 150
REQUEST 151
REQUEST 152
REQUEST 153
REQUEST 154
REQUEST 155
REQUEST 156
REQUEST 157
REQUEST 158
REQUEST 159
REQUEST 160
REQUEST 161
REQUEST 162
REQUEST 163
REQUEST 164
REQUEST 165
REQUEST 166
REQUEST 167
REQUEST 168
REQUEST 169
REQUEST 170
REQUEST 171
REQUEST 172
REQUEST 173
REQUEST 174
REQUEST 175
REQUEST 176
REQUEST 177
REQUEST 178
REQUEST 179
REQUEST 180
REQUEST 181
REQUEST 182
REQUEST 183
REQUEST 184
REQUEST 185
REQUEST 186
REQUEST 187
REQUEST 188
REQUEST 189
REQUEST 190
REQUEST 191
REQUEST 192
REQUEST 193
REQUEST 194
REQUEST 195
REQUEST 196
REQUEST 197
REQUEST 198
REQUEST 199
REQUEST 200
REQUEST 201
REQUEST 202
REQUEST 203
REQUEST 204
REQUEST 205
REQUEST 206
REQUEST 207
REQUEST 208
REQUEST 209
REQUEST 210
REQUEST 211
REQUEST 212
REQUEST 213
REQUEST 214
REQUEST 215
REQUEST 216
REQUEST 217
REQUEST 218
REQUEST 219
REQUEST 220
REQUEST 221
REQUEST 222

; Request 223 is set to be the spurious IRQ
;REQUEST 223
global Sharpen_Arch_IDT_Request223_0
Sharpen_Arch_IDT_Request223_0:
    iret