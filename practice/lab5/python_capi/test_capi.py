import sys
import os
import numpy as np

sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))

import mylib_api

def dot_product(a, b):
    return mylib_api.dot_product(a, b)