import numpy as np

def mae_custom(y_true, y_pred):
    return np.mean(np.abs(np.array(y_true) - np.array(y_pred)))

def mse_custom(y_true, y_pred):
    return np.mean((np.array(y_true) - np.array(y_pred)) ** 2)

def rmse_custom(y_true, y_pred):
    return np.sqrt(mse_custom(y_true, y_pred))

def mape_custom(y_true, y_pred):
    y_true, y_pred = np.array(y_true), np.array(y_pred)
    # Простая защита от деления на ноль: считаем только где факт != 0
    mask = y_true != 0
    if not np.any(mask):
        return 0.0
    return np.mean(np.abs((y_true[mask] - y_pred[mask]) / y_true[mask])) * 100

def r2_custom(y_true, y_pred):
    y_true, y_pred = np.array(y_true), np.array(y_pred)
    ss_res = np.sum((y_true - y_pred) ** 2)
    ss_tot = np.sum((y_true - np.mean(y_true)) ** 2)
    if ss_tot == 0:
        return 0.0
    return 1 - (ss_res / ss_tot)

def custom_calculate_metrics(y_true, y_pred):
    return {
        'R^2': r2_custom(y_true, y_pred),
        'MSE': mse_custom(y_true, y_pred),
        'RMSE': rmse_custom(y_true, y_pred),
        'MAE': mae_custom(y_true, y_pred),
        'MAPE': mape_custom(y_true, y_pred)
    }