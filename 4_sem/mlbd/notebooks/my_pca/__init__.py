# File: my_pca.py
import numpy as np
from typing import Tuple, Optional

class MyPCA:
    def __init__(self, n_components: Optional[int] = None):
        self.n_components = n_components
        self.components_ = None
        self.explained_variance_ = None
        self.explained_variance_ratio_ = None
        self.mean_ = None
        self.n_components_ = None
        
    def fit(self, X: np.ndarray) -> 'MyPCA':

        self.mean_ = np.mean(X, axis=0)
        
        X_centered = X - self.mean_
        
        n_samples = X.shape[0]
        cov_matrix = np.dot(X_centered.T, X_centered) / (n_samples - 1)
        
        eigenvalues, eigenvectors = np.linalg.eigh(cov_matrix)
        
        idx = np.argsort(eigenvalues)[::-1]
        eigenvalues = eigenvalues[idx]
        eigenvectors = eigenvectors[:, idx]
        
        if self.n_components is None:
            self.n_components_ = min(X.shape)
        elif isinstance(self.n_components, float) and 0 < self.n_components <= 1:
            cumsum = np.cumsum(eigenvalues) / np.sum(eigenvalues)
            self.n_components_ = np.searchsorted(cumsum, self.n_components) + 1
            self.n_components_ = min(self.n_components_, len(eigenvalues))
        else:
            self.n_components_ = int(self.n_components)
        
        self.components_ = eigenvectors[:, :self.n_components_].T
        self.explained_variance_ = eigenvalues[:self.n_components_]
        total_variance = np.sum(eigenvalues)
        self.explained_variance_ratio_ = self.explained_variance_ / total_variance
        
        return self
    
    def transform(self, X: np.ndarray) -> np.ndarray:
        if self.components_ is None:
            raise ValueError("Модель не обучена. Вызовите fit() перед transform()")
        
        X_centered = X - self.mean_
        return np.dot(X_centered, self.components_.T)
    
    def fit_transform(self, X: np.ndarray) -> np.ndarray:
        self.fit(X)
        return self.transform(X)
    
    def get_covariance_matrix(self, X: np.ndarray) -> np.ndarray:
        X_centered = X - np.mean(X, axis=0)
        n_samples = X.shape[0]
        return np.dot(X_centered.T, X_centered) / (n_samples - 1)
    
    def reconstruct(self, X_transformed: np.ndarray) -> np.ndarray:
        return np.dot(X_transformed, self.components_) + self.mean_
    
    def get_reconstruction_error(self, X: np.ndarray) -> float:
        X_transformed = self.transform(X)
        X_reconstructed = self.reconstruct(X_transformed)
        return np.mean((X - X_reconstructed) ** 2)


def compare_pca_implementations(X: np.ndarray, n_components: int = 2) -> dict:
    from sklearn.decomposition import PCA as SklearnPCA
    
    my_pca = MyPCA(n_components=n_components)
    X_my = my_pca.fit_transform(X)
    
    sklearn_pca = SklearnPCA(n_components=n_components)
    X_sklearn = sklearn_pca.fit_transform(X)
    
    results = {
        'my_pca': {
            'transformed': X_my,
            'components': my_pca.components_,
            'explained_variance': my_pca.explained_variance_,
            'explained_variance_ratio': my_pca.explained_variance_ratio_
        },
        'sklearn_pca': {
            'transformed': X_sklearn,
            'components': sklearn_pca.components_,
            'explained_variance': sklearn_pca.explained_variance_,
            'explained_variance_ratio': sklearn_pca.explained_variance_ratio_
        },
        'comparison': {
            'transform_diff': np.abs(X_my - X_sklearn).mean(),
            'components_diff': np.abs(my_pca.components_ - sklearn_pca.components_).mean(),
            'variance_diff': np.abs(my_pca.explained_variance_ - sklearn_pca.explained_variance_).mean(),
            'variance_ratio_diff': np.abs(my_pca.explained_variance_ratio_ - sklearn_pca.explained_variance_ratio_).mean()
        }
    }
    
    return results