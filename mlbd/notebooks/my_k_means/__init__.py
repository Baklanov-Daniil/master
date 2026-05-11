import numpy as np

class MyKMeans:
    def __init__(self, n_clusters=3, n_init=10, max_iter=300, random_state=None, tol=1e-4):
        self.n_clusters = n_clusters
        self.n_init = n_init
        self.max_iter = max_iter
        self.random_state = random_state
        self.tol = tol
        self.inertia_ = None
        self.labels_ = None
        self.cluster_centers_ = None

    def fit_predict(self, X):
        np.random.seed(self.random_state)
        best_labels = None
        best_inertia = float('inf')
        best_centroids = None

        for _ in range(self.n_init):
            initial_indices = np.random.choice(X.shape[0], self.n_clusters, replace=False)
            centroids = X[initial_indices].copy() # .copy() на случай, если X изменяемый

            for i in range(self.max_iter):
                distances = ((X - centroids[:, np.newaxis])**2).sum(axis=2)
                new_labels = np.argmin(distances, axis=0)

                new_centroids = np.array([X[new_labels == k].mean(axis=0) for k in range(self.n_clusters)])

                centroid_shift = np.linalg.norm(centroids - new_centroids)
                centroids = new_centroids
                if centroid_shift < self.tol:
                    break

            final_distances = ((X - centroids[new_labels])**2).sum(axis=1)
            inertia = np.sum(final_distances)

            if inertia < best_inertia:
                best_inertia = inertia
                best_labels = new_labels.copy()
                best_centroids = centroids.copy()

        self.inertia_ = best_inertia
        self.labels_ = best_labels
        self.cluster_centers_ = best_centroids
        
        return best_labels