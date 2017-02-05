global Sharpen_Mem_Memory_Memcpy_3void__void__int32_t_
Sharpen_Mem_Memory_Memcpy_3void__void__int32_t_:
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
    and eax, 3
    shr ecx, 2
    
    ; First part (as 32bit ints)
    cld
    rep movsd

    ; Second part (as bytes)
    mov ecx, eax
    rep movsb

    ; Done
    pop esi
    pop edi
    ret

global Sharpen_Mem_Memory_Memset_3void__int32_t_int32_t_
Sharpen_Mem_Memory_Memset_3void__int32_t_int32_t_:
    push edi
    
    mov ecx, [esp + 16] ; Count
    mov eax, [esp + 12] ; Value
    mov edi, [esp + 8]  ; Destination

    cld
    rep stosb

    pop edi
    ret