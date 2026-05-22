import numpy as np, os
rng = np.random.default_rng(67)
a = rng.standard_normal((100_000, 10)).astype(np.float64)
b = rng.standard_normal((100_000, 10)).astype(np.float64)
path = os.path.dirname(os.path.abspath(__file__))
np.save(os.path.join(path, "vectors_a.npy"), a)
np.save(os.path.join(path, "vectors_b.npy"), b)