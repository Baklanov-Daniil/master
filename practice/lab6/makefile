.PHONY: all lib test-all benchmark train predict csharp clean help

ifeq ($(OS),Windows_NT)
    LIB = c_core/mylib.dll
    CC = gcc
    CFLAGS = -shared -O2
    PYTHON = python
    DOTNET = dotnet
else
    LIB = c_core/libmylib.so
    CC = gcc
    CFLAGS = -shared -fPIC -O2
    PYTHON = python3
    DOTNET = dotnet
endif

# ================= ОСНОВНЫЕ ЦЕЛИ =================

all: lib train
	@echo "✅ Сборка C-библиотеки и обучение модели завершены."

# ---------- Лабораторная №5 ----------
$(LIB): c_core/mylib.c c_core/mylib.h
	$(CC) $(CFLAGS) -o $@ $<

lib: $(LIB)

test-ctypes: lib
	cd python_ctypes && $(PYTHON) test_ctypes.py

test-cffi: lib
	cd python_cffi && $(PYTHON) test_cffi.py

test-capi: lib
	cd python_capi && $(PYTHON) setup.py build_ext --inplace && $(PYTHON) test_capi.py

test-cython: lib
	cd python_cython && $(PYTHON) setup_cython.py build_ext --inplace && $(PYTHON) test_cython.py

test-all: test-ctypes test-cffi test-capi test-cython

benchmark: lib
	cd benchmark && $(PYTHON) data_generator.py && $(PYTHON) benchmark.py

# ---------- Лабораторная №6 ----------
train:
	cd ml_pipeline && $(PYTHON) train_model.py

predict:
	cd ml_pipeline && $(PYTHON) predict.py 50 50

csharp:
	cd csharp_client && $(DOTNET) run

# ---------- Очистка ----------
clean:
ifeq ($(OS),Windows_NT)
	-del /Q c_core\mylib.dll 2>nul
	-rmdir /S /Q python_capi\build python_cython\build 2>nul
	-del /Q python_capi\*.pyd python_cython\*.pyd 2>nul
	-del /Q benchmark\results.csv benchmark\*.npy benchmark\plot.png 2>nul
	-del /Q ml_pipeline\model.pkl 2>nul
else
	rm -f c_core/libmylib.so
	rm -rf python_capi/build python_cython/build python_capi/*.so python_cython/*.so
	rm -f benchmark/results.csv benchmark/*.npy benchmark/plot.png
	rm -f ml_pipeline/model.pkl
endif
	@echo  Очистка завершена."

