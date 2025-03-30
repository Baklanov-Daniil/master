# 1) алгоритм Прима
# 2) алгоритм Краскал

def get_data():
    data = []
    with open("data.txt", "r") as file:
        for i in file:
            data.append(list(map(int, i.split())))
    return data

def get_vertexes(data):
    vertexes_list = list(range(len(data)))
    return vertexes_list

def first_algorithm(data):
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
            min_ost_tree.append(min_weight)
            visited.append(to_vertex)
            not_visited.remove(to_vertex)
        else:
            break
    print(sum(min_ost_tree))
        
            

def second_algorithm(data):

    def find_index(components, find_value):
        for component_id in range(len(components)):
            for value in components[component_id]:
                if value == find_value:
                    return component_id
        return None


    vertexes_dict = dict()
    for row in range(len(data)):
        for column in range(row+1, len(data[row])):
            if data[row][column]!= 0:
                vertexes_dict[row+1,column+1] = data[row][column]
    vertexes_dict = dict(sorted(vertexes_dict.items(), key=lambda item: item[1]))
    sum = 0
    components = [[]]
    components[0].append(list(vertexes_dict.keys())[0][0])
    for vertexes_pair in vertexes_dict.items():
        id_1 = find_index(components, vertexes_pair[0][0])
        id_2 = find_index(components, vertexes_pair[0][1])
        if id_1==None and id_2==None:
            components.append(list())
            components[-1].append(vertexes_pair[0][0])
            components[-1].append(vertexes_pair[0][1])
        else: 
            if id_1!=None and id_2==None:
                components[id_1].append(vertexes_pair[0][1])
            elif id_1==None and id_2!=None:       
                components[id_2].append(vertexes_pair[0][0])
            elif id_1!=id_2:
                components[id_1].extend(components[id_2])
                del components[id_2]
            else: 
                continue
        sum+=vertexes_pair[1]
    print(sum)

        
        
        

if __name__=="__main__":
    data = get_data()
    first_algorithm(data)
    second_algorithm(data)
