def get_data():
    data = []
    with open("data.txt", "r") as file:
        for line in file:
            data.append(list(map(int, line.split())))
    
    for i in range(len(data)):
        for j in range(len(data)):
            if data[i][j] == 0:
                data[i][j] = float('inf')
    return data

def ford_fulkerson(capacity_matrix, source, sink):
    num_nodes = len(capacity_matrix)
    
    residual = [[0 if capacity_matrix[i][j] == float('inf') else capacity_matrix[i][j] 
                for j in range(num_nodes)] 
               for i in range(num_nodes)]
    
    parent = [-1] * num_nodes
    max_flow = 0

    def dfs(start):
        visited = [False] * num_nodes
        stack = [(start, float('inf'))]
        visited[start] = True

        while stack:
            u, current_flow = stack.pop()
            for v in range(num_nodes):
                if not visited[v] and residual[u][v] > 0:
                    visited[v] = True
                    parent[v] = u
                    new_flow = min(current_flow, residual[u][v])
                    if v == sink:
                        return new_flow
                    stack.append((v, new_flow))
        return 0

    while True:
        path_flow = dfs(source)
        if path_flow == 0:
            break
        
        v = sink
        while v != source:
            u = parent[v]
            residual[u][v] -= path_flow
            residual[v][u] += path_flow
            v = u
        
        max_flow += path_flow

    return max_flow

if __name__ == "__main__":
    capacity_matrix = get_data()

    SOURCE = 0
    SINK = 5

    result = ford_fulkerson(capacity_matrix, SOURCE, SINK)
    print(f"Максимальный поток из вершины {SOURCE} в вершину {SINK}: {result}")