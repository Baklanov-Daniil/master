# флойд
import copy
def get_data():
    data = []
    with open("data.txt", "r") as file:
        for i in file:
            data.append(list(map(int, i.split())))
    for i in range(len(data)):
        for j in range(len(data)):
            if data[i][j]==0:
                data[i][j] = float("inf")
    return data

def get_vertexes(data):
    vertexes_list = list(range(len(data)))
    return vertexes_list

def show_data(data):
    data_f = copy.deepcopy(data)
    for i in range(len(data)):
        data_f[i][i] = "0"
        for j in range(len(data_f)):
            if data_f[i][j] == float("inf"):
                data_f[i][j] = "∞"
            print(f"{data_f[i][j]} ", end="")
        print()    

def algorithm_floida(data):
    not_visited = get_vertexes(data)
    for base_vertex in not_visited:
        for first_vertex in range(len(data)):
            for second_vertex in range(len(data)):
                if data[first_vertex][second_vertex]>data[first_vertex][base_vertex] + data[base_vertex][second_vertex]:
                    data[first_vertex][second_vertex] = data[first_vertex][base_vertex] + data[base_vertex][second_vertex]
        show_data(data)
        print()

if __name__=="__main__":
    data = get_data()
    algorithm_floida(data)
