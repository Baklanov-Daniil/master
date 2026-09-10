#define PY_SSIZE_T_CLEAN
#include <Python.h>
#include <stdlib.h>
#include "mylib.h"

static int* extract_array(PyObject* obj, double** out_arr, Py_ssize_t* length) {
    if (!obj) return -1;
    
    PyObject* fast = PySequence_Fast(obj, "Argument must be iterable");
    if (!fast) return -1;
    
    *length = PySequence_Fast_GET_SIZE(fast);
    if (*length == 0) {
        Py_DECREF(fast);
        return -1;
    }

    // Выделяем память
    *out_arr = (double*)malloc(*length * sizeof(double));
    if (!*out_arr) {
        Py_DECREF(fast);
        return -1;
    }

    // Заполняем массив
    for (Py_ssize_t i = 0; i < *length; i++) {
        PyObject* item = PySequence_Fast_GET_ITEM(fast, i);
        (*out_arr)[i] = PyFloat_AsDouble(item);
        if ((*out_arr)[i] == -1.0 && PyErr_Occurred()) {
            free(*out_arr);
            Py_DECREF(fast);
            return -1;
        }
    }
    
    Py_DECREF(fast);
    return 0;
}

static PyObject* py_dot_product(PyObject* self, PyObject* args) {
    PyObject *a_obj, *b_obj;

    if (!PyArg_ParseTuple(args, "OO", &a_obj, &b_obj)) {
        return NULL;
    }

    double *a = NULL, *b = NULL;
    Py_ssize_t len_a = 0, len_b = 0;

    if (extract_array(a_obj, &a, &len_a) != 0) {
        PyErr_SetString(PyExc_ValueError, "Invalid input for vector a");
        goto cleanup;
    }

    if (extract_array(b_obj, &b, &len_b) != 0) {
        PyErr_SetString(PyExc_ValueError, "Invalid input for vector b");
        goto cleanup;
    }

    if (len_a != len_b) {
        PyErr_SetString(PyExc_ValueError, "Vectors must have the same length");
        goto cleanup;
    }

    double result;
    if (dot_product(a, b, (size_t)len_a, &result) != 0) {
        PyErr_SetString(PyExc_RuntimeError, "dot_product failed internally");
        goto cleanup;
    }

    free(a);
    free(b);
    return PyFloat_FromDouble(result);

cleanup:
    if (a) free(a);
    if (b) free(b);
    return NULL;
}

static PyMethodDef mylib_methods[] = {
    {"dot_product", py_dot_product, METH_VARARGS, "Calculate scalar product of two vectors."},
    {NULL, NULL, 0, NULL}
};

static struct PyModuleDef mylib_module = {
    PyModuleDef_HEAD_INIT,
    "mylib_api",
    "C API Wrapper",
    -1,
    mylib_methods
};

PyMODINIT_FUNC PyInit_mylib_api(void) {
    return PyModule_Create(&mylib_module);
}