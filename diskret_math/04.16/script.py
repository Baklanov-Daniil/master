# алгоритм ветвей и границ (алгоритм Литтла)
import copy
def get_data():
    data = []
    with open("data.txt", "r") as file:
        for i in file:
            data.append(list(map(int, i.split())))
    for i in range(len(data)):
        if i==i:
            data[i][i] = float("inf")
    return data

def get_vertexes(data):
    vertexes_list = list(range(len(data)))
    return vertexes_list

def show_data(data):
    data_copy = copy.deepcopy(data)
    for i in range(len(data_copy)):
        for j in range(len(data_copy[i])):
            if data_copy[i][j] == float("inf"):
                data_copy[i][j] = "∞"
            print(f"{data_copy[i][j]} ", end="")
        print()    
    print()

def algorithm_Littla(data):
    def get_min_string(data, param=None):
        if param:
            i_0 = param[0]
            j_0 = param[1]
            min_elem = float("inf")
            for j in range(len(data[i_0])):
                if j!=j_0:
                    min_elem = min(min_elem, data[i_0][j])
            return min_elem
        else:
            min_list = list()
            for i in range(len(data)):
                min_elem = float("inf")
                for j in range(len(data[i])):
                    min_elem = min(min_elem, data[i][j])
                min_list.append(min_elem)
        return min_list
    
    def get_min_column(data, param=None):
        if param:
            i_0 = param[0]
            j_0 = param[1]
            min_elem = float("inf")
            for i in range(len(data[j_0])):
                if i!=i_0:
                    min_elem = min(min_elem, data[i][j_0])
            return min_elem
        else:
            min_list = list()
            min_elem = float("inf")
            for i in range(len(data)):
                min_elem = float("inf")
                for j in range(len(data[i])):
                    min_elem = min(min_elem, data[j][i])
                min_list.append(min_elem)
            return min_list
    
    def mins_value_string(data, min_list):
        for i in range(len(data)):
            for j in range(len(data[i])):
                
                data[i][j] -= min_list[i]

    def mins_value_column(data, min_list):
        for i in range(len(data)):
            for j in range(len(data[i])):
                data[j][i] -= min_list[i]

    def string_reduction(data):
        min_list = get_min_string(data)
        mins_value_string(data, min_list)      
        return min_list     

    def column_reduction(data):
        min_list = get_min_column(data)
        mins_value_column(data, min_list)  
        return min_list   
    
    def get_zero_mark(data):
        data_zero = copy.deepcopy(data)
        for i in range(len(data)):
            for j in range(len(data)):
                data_zero[i][j] = 0
        for i in range(len(data)):
            for j in range(len(data)):
                if data[i][j] == 0:
                    min_elem_string = get_min_string(data, [i,j])
                    min_elem_column = get_min_column(data, [i,j])
                    data_zero[i][j] = min_elem_string+ min_elem_column
        min_elem_id = find_min(data_zero)
        data[min_elem_id[0]][min_elem_id[1]] = float("inf")
        data[min_elem_id[1]][min_elem_id[0]] = float("inf")
        return min_elem_id

    def find_min(data):
        max_elem = 0
        elem_id = list()
        for i in range(len(data)):
            for j in range(len(data)):
                if data[i][j]!=0 and max_elem<data[i][j]:
                    max_elem = data[i][j]
                    elem_id = [i,j]
        
        return elem_id

    def make_new_data(data, elem_id):
        new_data = list()
        for i in range(len(data)):
            new_data.append(list())
            for j in range(len(data)):
                if i!=elem_id[0] and j!=elem_id[1]:
                    new_data[i].append(data[i][j])
        new_data.remove(list())
        return new_data
    
    min_bound = 0
    result_value = list()
    main_data = copy.deepcopy(data)
    while data:
        step1 = string_reduction(data)
        step2 = column_reduction(data)
        min_bound += sum(step1) + sum(step2)
        zero_id = get_zero_mark(data)
        result_value.append(main_data[zero_id[0]][zero_id[1]])
        data = make_new_data(data, zero_id)
        main_data = make_new_data(main_data, zero_id)
    print(sum(result_value))
    print(min_bound)
        
if __name__=="__main__":
    data = get_data()
    algorithm_Littla(data)
