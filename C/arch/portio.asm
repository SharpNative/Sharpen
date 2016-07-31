; Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_(ushort port, byte value)
global Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_
Sharpen_Arch_PortIO_Out8_2uint16_t_uint8_t_:
    mov dx, [esp + 1 * 4]
    mov al, [esp + 2 * 4]
    out dx, al
    ret

; Sharpen_Arch_PortIO_In8_1uint16_t_(ushort port)
global Sharpen_Arch_PortIO_In8_1uint16_t_
Sharpen_Arch_PortIO_In8_1uint16_t_:
    mov dx, [esp + 1 * 4]
    in byte al, dx
    ret

; Sharpen_Arch_PortIO_Out16_2uint16_t_uint16_t_(ushort port, ushort value)
global Sharpen_Arch_PortIO_Out16_2uint16_t_uint16_t_
Sharpen_Arch_PortIO_Out16_2uint16_t_uint16_t_:
    mov dx, [esp + 1 * 4]
    mov ax, [esp + 2 * 4]
    out dx, ax
    ret

; Sharpen_Arch_PortIO_In16_1uint16_t_(ushort port)
global Sharpen_Arch_PortIO_In16_1uint16_t_
Sharpen_Arch_PortIO_In16_1uint16_t_:
    mov dx, [esp + 1 * 4]
    in word ax, dx
    ret

; Sharpen_Arch_PortIO_Out32_2uint16_t_uint32_t_(ushort port, uint value)
global Sharpen_Arch_PortIO_Out32_2uint16_t_uint32_t_
Sharpen_Arch_PortIO_Out32_2uint16_t_uint32_t_:
    mov dx, [esp + 1 * 4]
    mov eax, [esp + 2 * 4]
    out dx, eax
    ret

; Sharpen_Arch_PortIO_In32_1uint16_t_(ushort port)
global Sharpen_Arch_PortIO_In32_1uint16_t_
Sharpen_Arch_PortIO_In32_1uint16_t_:
    mov dx, [esp + 1 * 4]
    in dword eax, dx
    ret