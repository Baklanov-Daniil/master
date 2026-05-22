from setuptools import setup, Extension
import os

current_dir = os.path.dirname(os.path.abspath(__file__))
parent_dir = os.path.join(current_dir, '..')

module = Extension(
    'mylib_api',
    sources=['module.c', os.path.join(parent_dir, 'c_core', 'mylib.c')],
    include_dirs=[os.path.join(parent_dir, 'c_core')],
    extra_compile_args=['/O2'] if os.name == 'nt' else ['-O2']
)

setup(
    name='mylib_api',
    version='1.0',
    ext_modules=[module]
)