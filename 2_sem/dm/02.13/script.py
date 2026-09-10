def get_data():
    data = []
    with open("data.txt", "r") as file:
        for i in file:
            data.append(list(map(int, i.split())))
    return data

def get_vertexes(data):
    vertexes_set = set(range(len(data)))
    return vertexes_set

def first_algorithm(data):
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
                if data[row][column] != 0 and column in vertex_list:
                    clasters[counter].append(column)
                    vertex_list.remove(column)
        counter += 1
    print(len(clasters))

def second_algorithm(data):
    vertexes_set = get_vertexes(data)
    vertexes_dict = {name: 0 for name in vertexes_set}
    vertexes_dict[0] = 1
    vertex = 3
    for vertex in list(vertexes_dict.keys())[1:]:
        min_set = set()
        for column in range(len(data[0])):
            if data[vertex][column] != 0 and vertexes_dict[column] != 0:
                min_set.add(vertexes_dict[column])
        try:        
            mn = min(min_set)
            vertexes_dict[vertex] = mn
            for check_vertex in vertexes_dict:
                if vertexes_dict[check_vertex]!=mn and vertexes_dict[check_vertex] in min_set:
                    vertexes_dict[check_vertex] = mn
        except:
            vertexes_dict[vertex] = max(vertexes_dict.values())+1
    print(len(set(vertexes_dict.values())))
        

if __name__=="__main__":
    data = get_data()
    first_algorithm(data)
    second_algorithm(data)


        