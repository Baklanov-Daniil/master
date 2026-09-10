.PHONY: lib test-all benchmark clean

ifeq ($(OS),Windows_NT)
    LIB = c_core/mylib.dll
    CC = gcc
    CFLAGS = -shared -O2
else
    LIB = c_core/libmylib.so
    CC = gcc
    CFLAGS = -shared -fPIC -O2
endif

$(LIB): c_core/mylib.c c_core/mylib.h
	$(CC) $(CFLAGS) -o $@ $<

lib: $(LIB)

test-ctypes: lib
	cd python_ctypes && python test_ctypes.py

test-cffi: lib
	cd python_cffi && python test_cffi.py

test-capi: lib
	cd python_capi && python setup.py build_ext --inplace && python test_capi.py

test-cython: lib
	cd python_cython && python setup_cython.py build_ext --inplace && python test_cython.py

test-all: test-ctypes test-cffi test-capi test-cython

all: test-ctypes test-cffi test-capi test-cython

benchmark:
	cd benchmark && python benchmark.py

clean:
ifeq ($(OS),Windows_NT)
	del /Q c_core\*.dll python_capi\*.pyd python_cython\*.pyd 2>nul
	rmdir /S /Q python_capi\build python_cython\build 2>nul
else
	rm -f c_core/*.so python_capi/*.so python_cython/*.so
	rm -rf python_capi/build python_cython/build
endif