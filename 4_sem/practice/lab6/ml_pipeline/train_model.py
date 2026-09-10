# ml_pipeline/train_model.py
import numpy as np
from sklearn.linear_model import LogisticRegression
from sklearn.preprocessing import StandardScaler
from sklearn.pipeline import Pipeline
import pickle
import os


np.random.seed(42)
X = np.random.rand(100, 2) * 100
y = (X[:, 0] + X[:, 1] > 100).astype(int)

model = Pipeline([
    ('scaler', StandardScaler()),
    ('classifier', LogisticRegression())
])

model.fit(X, y)

model_path = os.path.join(os.path.dirname(__file__), 'model.pkl')
with open(model_path, 'wb') as f:
    pickle.dump(model, f)
