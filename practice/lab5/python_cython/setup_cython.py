from setuptools import setup, Extension
from Cython.Build import cythonize
import numpy
import os

current_dir = os.path.dirname(os.path.abspath(__file__))
parent_dir = os.path.join(current_dir, '..')
c_core_dir = os.path.join(parent_dir, 'c_core')

ext = Extension(
    name="cy_wrapper",
    sources=["wrapper.pyx", os.path.join(c_core_dir, "mylib.c")],
    include_dirs=[numpy.get_include(), c_core_dir],
    extra_compile_args=['/O2'] if os.name == 'nt' else ['-O2']
)

setup(
    name="cy_wrapper",
    ext_modules=cythonize([ext], language_level=3),
    zip_safe=False
)