# ml_pipeline/predict.py
import sys
import json
import pickle
import os
import numpy as np


if len(sys.argv) != 3:
    print(json.dumps({"error": "Invalid args. Usage: predict.py <feat1> <feat2>"}))
    sys.exit(1)

try:
    raw_input = [float(sys.argv[1]), float(sys.argv[2])]
except ValueError:
    print(json.dumps({"error": "Arguments must be numbers"}))
    sys.exit(1)

X_new = np.array([raw_input])

model_path = os.path.join(os.path.dirname(__file__), 'model.pkl')
if not os.path.exists(model_path):
    print(json.dumps({"error": "Model file not found"}))
    sys.exit(1)

with open(model_path, 'rb') as f:
    model = pickle.load(f)

prediction = model.predict(X_new)
probability = model.predict_proba(X_new)[0]

result = {
    "input": raw_input,
    "class": int(prediction[0]),
    "probabilities": [float(probability[0]), float(probability[1])]
}
print(json.dumps(result))
