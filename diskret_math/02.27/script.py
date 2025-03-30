# на вход подается граф (неорентировынный) необходимо определить в графе все ребра которые являеются мостами 
# мост - это ребро при удалени которого количество компонент свяности увеличивается
def get_data():
    data = []
    with open("data.txt", "r") as file:
        for i in file:
            data.append(list(map(int, i.split())))
    return data

def get_vertexes(data):
    vertexes_list = list(range(len(data)))
    return vertexes_list

def algorithm_prima(data):
    not_visited = (get_vertexes(data))
    visited = []
    min_ost_tree = []
    visited.append(not_visited.pop(0))
    from_vertex = to_vertex = None
    while not_visited:
        min_weight = float('inf')
        for first_vertex in visited:
            for second_vertex in not_visited:
                if data[first_vertex][second_vertex] != 0 and data[first_vertex][second_vertex]<min_weight:
                    min_weight = data[first_vertex][second_vertex]
                    from_vertex, to_vertex = first_vertex, second_vertex
        if from_vertex!=None and to_vertex!=None:
            min_ost_tree.append([from_vertex, to_vertex])
            visited.append(to_vertex)
            not_visited.remove(to_vertex)
        else:
            break
    return min_ost_tree

def find_components(data, edge=None):
    vertexes_set = get_vertexes(data)
    vertex_list = list(vertexes_set)
    clasters = list()
    counter = 0
    while vertex_list:
        clasters.append(list())
        clasters[counter].append(vertex_list[0])
        del vertex_list[0]
        for row in clasters[counter]:
            for column in range(len(data[0])):
                if edge and row == edge[0] and column == edge[1]:
                    continue
                if data[row][column] == 1 and column in vertex_list:
                    clasters[counter].append(column)
                    vertex_list.remove(column)
        counter += 1
    return len(clasters)

def find_bridge(data, ost_tree):
    for edge in ost_tree:
        if find_components(data, edge)!=1:
            print(f"ребро {edge} является мостом")



if __name__=="__main__":
    data = get_data()
    ost_tree = algorithm_prima(data)
    find_bridge(data, ost_tree)
