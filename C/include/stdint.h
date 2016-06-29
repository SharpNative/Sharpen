#ifndef __STDINT_H
#define __STDINT_H

/* Size type */
typedef unsigned long size_t;

/* Integer types */
typedef signed char int8_t;
typedef short int16_t;
typedef int int32_t;
typedef long long int64_t;

typedef unsigned char uint8_t;
typedef unsigned short uint16_t;
typedef unsigned int uint32_t;
typedef unsigned long long uint64_t;

/* Types for pointers */
typedef long intptr_t;
typedef unsigned long uintptr_t;

/* Types for largest integers */
typedef long long intmax_t;
typedef unsigned long long uintmax_t;

/* Minimum & maximum */
#define INT8_MIN     (-128)
#define INT16_MIN    (-32767 - 1)
#define INT32_MIN    (-2147483647 - 1)
#define INT64_MIN    (-9223372036854775807i64 - 1)

#define INT8_MAX     (127)
#define INT16_MAX    (32767)
#define INT32_MAX    (2147483647)
#define INT64_MAX    (9223372036854775807i64)

#define UINT8_MAX    (0xffui8)
#define UINT16_MAX   (0xffffui16)
#define UINT32_MAX   (0xffffffffui32)
#define UINT64_MAX   (0xffffffffffffffffui64)

#define INTMAX_MIN   INT64_MIN
#define INTMAX_MAX   INT64_MAX
#define UINTMAX_MAX  UINT64_MAX

#define SIZE_MAX     UINT32_MAX

#endif