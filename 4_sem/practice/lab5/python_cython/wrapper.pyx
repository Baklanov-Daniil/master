cimport numpy as cnp
from libc.stddef cimport size_t

# Объявляем внешнюю C-функцию из mylib.h
cdef extern from "../c_core/mylib.h":
    int dot_product(const double* a, const double* b, size_t length, double* result)

# Python-функция для вызова
def py_dot_product(cnp.ndarray[cnp.float64_t, ndim=1] a, 
                   cnp.ndarray[cnp.float64_t, ndim=1] b):
    """
    Вычисляет скалярное произведение двух векторов.
    """
    cdef double res = 0.0
    cdef size_t n = a.shape[0]
    
    # Проверка длин
    if b.shape[0] != n:
        raise ValueError("Vectors must have the same length")
    
    cdef int status = dot_product(&a[0], &b[0], n, &res)
    
    if status != 0:
        raise RuntimeError("dot_product failed")
    
    return res