#алгоритм Дейкстра
def algorithm_deikstra(data, start, end):
    data_len = len(data)
    dist = [float('inf')] * data_len
    dist[start] = 0
    visited = [False] * data_len
    for _ in range(data_len):
        first_vertex = -1
        min_dist = float('inf')
        for i in range(data_len):
            if not visited[i] and dist[i] < min_dist:
                min_dist = dist[i]
                first_vertex = i
        if first_vertex == -1 or first_vertex == end:
            break
        visited[first_vertex] = True
        for second_vertex in range(data_len):
            if data[first_vertex][second_vertex] > 0:
                new_dist = dist[first_vertex] + data[first_vertex][second_vertex]
                if new_dist < dist[second_vertex]:
                    dist[second_vertex] = new_dist
    print(dist[end])


def get_data():
    data = []    
    with open("data.txt", "r") as file:
        for i in file:
            data.append(list(map(int, i.split())))
    return data

if __name__=="__main__":
    data = get_data()
    algorithm_deikstra(data, 0, 4)
    