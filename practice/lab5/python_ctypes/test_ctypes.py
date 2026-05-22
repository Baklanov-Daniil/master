import ctypes
import sys
import numpy as np
import os

lib_path = os.path.join(
    os.path.dirname(os.path.abspath(__file__)),
    '..', 'c_core',
    'mylib.dll' if sys.platform.startswith('win') else 'libmylib.so'
)
lib_path = os.path.abspath(lib_path)

lib = ctypes.CDLL(lib_path)

lib.dot_product.argtypes = [
    ctypes.POINTER(ctypes.c_double),
    ctypes.POINTER(ctypes.c_double),
    ctypes.c_size_t,
    ctypes.POINTER(ctypes.c_double)
]
lib.dot_product.restype = ctypes.c_int

def dot_product(a, b):
    res = ctypes.c_double()
    a_ptr = a.ctypes.data_as(ctypes.POINTER(ctypes.c_double))
    b_ptr = b.ctypes.data_as(ctypes.POINTER(ctypes.c_double))
    lib.dot_product(a_ptr, b_ptr, len(a), ctypes.byref(res))
    return res.value