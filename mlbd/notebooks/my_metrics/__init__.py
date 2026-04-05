import numpy as np

__all__ = ["mae_custom", "mse_custom", "rmse_custom", "mape_custom", "r2_custom", "custom_calculate_metrics"]

def mae_custom(y_true, y_pred):
    return np.mean(np.abs(y_true - y_pred))

def mse_custom(y_true, y_pred):
    return np.mean((y_true - y_pred) ** 2)

def rmse_custom(y_true, y_pred):
    return np.sqrt(mse_custom(y_true, y_pred))

def mape_custom(y_true, y_pred):
    return np.mean(np.abs((y_true - y_pred) / y_true))

def r2_custom(y_true, y_pred):
    ss_res = np.sum((y_true - y_pred) ** 2)
    ss_tot = np.sum((y_true - np.mean(y_true)) ** 2)
    return 1 - (ss_res / ss_tot)

def custom_calculate_metrics(y_true, y_pred):
    return {
        'R^2': r2_custom(y_true, y_pred),
        'MSE': mse_custom(y_true, y_pred),
        'RMSE': rmse_custom(y_true, y_pred),
        'MAE': mae_custom(y_true, y_pred),
        'MAPE': mape_custom(y_true, y_pred)
    }