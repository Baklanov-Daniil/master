import numpy as np

__all__ = ["get_confusion_matrix_elements", "my_accuracy", "my_precision", "my_precision", "my_recall", "my_f1", "my_confusion_matrix"]

def get_confusion_matrix_elements(y_true, y_pred):

    y_true = np.array(y_true)
    y_pred = np.array(y_pred)
    
    tp = np.sum((y_true == 1) & (y_pred == 1))
    tn = np.sum((y_true == 0) & (y_pred == 0))
    fp = np.sum((y_true == 0) & (y_pred == 1))
    fn = np.sum((y_true == 1) & (y_pred == 0))
    
    return int(tp), int(tn), int(fp), int(fn)

def my_accuracy(y_true, y_pred):
    tp, tn, fp, fn = get_confusion_matrix_elements(y_true, y_pred)
    return (tp + tn) / (tp + tn + fp + fn)

def my_precision(y_true, y_pred):
    tp, tn, fp, fn = get_confusion_matrix_elements(y_true, y_pred)
    if (tp + fp) == 0: return 0.0
    return tp / (tp + fp)

def my_recall(y_true, y_pred):
    tp, tn, fp, fn = get_confusion_matrix_elements(y_true, y_pred)
    if (tp + fn) == 0: return 0.0
    return tp / (tp + fn)

def my_f1(y_true, y_pred):
    p = my_precision(y_true, y_pred)
    r = my_recall(y_true, y_pred)
    if (p + r) == 0: return 0.0
    return 2 * (p * r) / (p + r)

def my_confusion_matrix(y_true, y_pred):
    tp, tn, fp, fn = get_confusion_matrix_elements(y_true, y_pred)
    return np.array([[tn, fp], [fn, tp]])