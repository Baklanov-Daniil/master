import sys, os, time, numpy as np
from statistics import mean, median, stdev
import pandas as pd
import matplotlib.pyplot as plt

# Пути
ROOT = os.path.abspath(os.path.join(os.path.dirname(__file__), ".."))
for f in ["python_ctypes", "python_cffi", "python_capi", "python_cython"]:
    sys.path.append(os.path.join(ROOT, f))

import test_ctypes, test_cffi, mylib_api, cy_wrapper

a = np.load(os.path.join(os.path.dirname(__file__), "vectors_a.npy"))
b = np.load(os.path.join(os.path.dirname(__file__), "vectors_b.npy"))

wrappers = {
    "ctypes": test_ctypes.dot_product,
    "cffi": test_cffi.dot_product,
    "C_API": lambda x, y: mylib_api.dot_product(list(x), list(y)),
    "Cython": cy_wrapper.py_dot_product
}

N_RUNS, N_CALLS = 50, 100_000
raw_data = []

# 1. Итерируемся по методам (оберткам), а не по запускам
for name, func in wrappers.items():
    # Разогрев для текущего метода (не считаем в статистику)
    for _ in range(1000): func(a[0], b[0])
    
    # 50 замеров по 100_000 вызовов
    for i in range(1, N_RUNS + 1):
        t0 = time.perf_counter()
        for j in range(N_CALLS): func(a[j], b[j])
        elapsed = time.perf_counter() - t0
        # Сохраняем в "длинном" формате: метод, номер запуска, время
        raw_data.append({"method": name, "run_id": i, "time": elapsed})

# 2. Создаем таблицу и считаем статистику через groupby
df = pd.DataFrame(raw_data)
df["time"] = df["time"].astype(float) # Гарантия, что время — это числа

stats = df.groupby("method")["time"].agg(["min", "max", "mean", "median", "std"]).round(4)

# 3. Вывод и сохранение
print(stats)
df.to_csv(os.path.join(os.path.dirname(__file__), "raw_runs.csv"), index=False)
stats.to_csv(os.path.join(os.path.dirname(__file__), "results.csv"))

plt.figure(figsize=(8, 5))
plt.bar(stats.index, stats["mean"], yerr=stats["std"], capsize=5)
plt.ylabel("Time (s)")
plt.savefig(os.path.join(os.path.dirname(__file__), "plot.png"))
