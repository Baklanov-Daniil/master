#include "mylib.h"

int dot_product(const double* a, const double* b, size_t length, double* result) {
    if (!a || !b || !result || length == 0) {
        return -1;
    }

    double sum = 0.0;
    for (size_t i = 0; i < length; ++i) {
        sum += a[i] * b[i];
    }

    *result = sum;
    return 0;
}