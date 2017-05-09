#ifndef __ACSHARPENEX_H__
#define __ACSHARPENEX_H__

#ifndef ACPI_USE_NATIVE_DIVIDE

#ifndef ACPI_DIV_64_BY_32
#define ACPI_DIV_64_BY_32(n_hi, n_lo, d32, q32, r32) __asm__ __volatile__("divl %2;" : "=a"(q32), "=d"(r32) : "r"(d32), "0"(n_lo), "1"(n_hi))
#endif

#ifndef ACPI_SHIFT_RIGHT_64
#define ACPI_SHIFT_RIGHT_64(n_hi, n_lo) __asm__ __volatile__("shrl $1, %2; rcrl $1, %3;" : "=r"(n_hi), "=r"(n_lo) : "0"(n_hi), "1"(n_lo))
#endif

#endif

#endif /* __ACSHARPENEX_H__ */