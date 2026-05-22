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
results = {}

for name, func in wrappers.items():
    print(f"прогрев {name}")
    for _ in range(1000): func(a[0], b[0])
    print(f"старт {name}")
    times = []
    for _ in range(N_RUNS):
        t0 = time.perf_counter()
        for i in range(N_CALLS): func(a[i], b[i])
        times.append(time.perf_counter() - t0)
    
    results[name] = {
        "min": min(times), "max": max(times),
        "mean": mean(times), "median": median(times), "std": stdev(times)
    }

df = pd.DataFrame(results).T
df = df.round(4)

print(df)

csv_path = os.path.join(os.path.dirname(__file__), "results.csv")
df.to_csv(csv_path)

plt.figure(figsize=(8, 5))
plt.bar(df.index, df["mean"], yerr=df["std"], capsize=5, 
        color=["#4e79a7","#f28e2b","#e15759","#76b7b2"])
plt.ylabel("Время (секунды)")
plt.title("Сравнение производительности FFI подходов\n(100,000 вызовов, 50 запусков)")
plt.grid(axis="y", alpha=0.3)
plt.tight_layout()
plot_path = os.path.join(os.path.dirname(__file__), "plot.png")
plt.savefig(plot_path, dpi=150)