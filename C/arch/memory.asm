global Sharpen_Memory_Memcpy_3void__void__int32_t_
Sharpen_Memory_Memcpy_3void__void__int32_t_:
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

global Sharpen_Memory_Memset_3void__int32_t_int32_t_
Sharpen_Memory_Memset_3void__int32_t_int32_t_:
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