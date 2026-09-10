from cffi import FFI
import sys
import os
import numpy as np

ffi = FFI()
ffi.cdef("""
    int dot_product(const double* a, const double* b, size_t length, double* result);
""")

lib_name = "mylib.dll" if sys.platform.startswith("win") else "libmylib.so"
lib_path = os.path.abspath(os.path.join(os.path.dirname(__file__), '..', 'c_core', lib_name))

lib = ffi.dlopen(lib_path)

def dot_product(a, b):
    result = ffi.new("double*")
    a_ptr = ffi.cast("double*", a.ctypes.data)
    b_ptr = ffi.cast("double*", b.ctypes.data)
    
    status = lib.dot_product(a_ptr, b_ptr, len(a), result)
    if status != 0:
        raise RuntimeError(f"C function returned error code: {status}")
    return result[0]