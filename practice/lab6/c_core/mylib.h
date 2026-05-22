#ifndef MYLIB_H
#define MYLIB_H

#include <stddef.h>


#ifdef _WIN32
    #define EXPORT __declspec(dllexport)
#else
    #define EXPORT __attribute__((visibility("default")))
#endif

#define FIXED_VECTOR_SIZE 10

EXPORT int dot_product(const double* a, const double* b, size_t length, double* result);

#endif