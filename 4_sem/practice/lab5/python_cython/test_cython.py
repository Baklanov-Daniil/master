import numpy as np
import cy_wrapper

def dot_product(a, b):
    return cy_wrapper.py_dot_product(a, b)