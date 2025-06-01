import copy

def show_data(data):
    data_copy = copy.deepcopy(data)
    for i in range(len(data_copy)):
        for j in range(len(data_copy[i])):
            if data_copy[i][j] == float("inf"):
                data_copy[i][j] = "∞"
            print(f"{data_copy[i][j]} \t", end="")
        print()    
    print()

def get_data():
    data = []
    with open("data.txt", "r") as file:
        for i in file:
            data.append(list(map(int, i.split())))
    for i in range(len(data)):
        if i==i:
            data[i][i] = float("inf")
    return data

def algorithm_littla(data):
    del_rows = list()
    del_columns = list()
    path_coast = 0 
    for _ in range(3):
        min_row, data = make_row_reduction(data)
        min_column, data = make_column_reduction(data)
        path_coast += sum(min_row) + sum(min_column)
        zero_data = make_zero_data(data)
        zero_id = find_max_zero(zero_data)
        del_rows.append(zero_id[0])
        del_columns.append(zero_id[1])
        data = make_matrix_reduction(data, zero_id)
        show_data(data)

    print(path_coast)

def get_min_row(data):
    return [min(data[i]) for i in range(len(data)) ]

def get_min_column(data):
    return [
        min([data[j][i]for j in range(len(data[i]))]) 
        for i in range(len(data))
    ]

def get_min_row_zero(data, i, j):
    return min([data[k][j] for k in range(len(data)) if k!=i])

def get_min_column_zero(data, i, j):
    return min([data[i][k] for k in range(len(data)) if k!=j])

def make_row_reduction(data):
    min_row = get_min_row(data)
    data = [
        [
            data[i][j]-min_row[i]
            for j in range(len(data[i]))
        ] 
        for i in range(len(data))
    ]
    return min_row, data    

def make_column_reduction(data):
    min_column = get_min_column(data)
    data = [
        [
            data[i][j]-min_column[j]
            for j in range(len(data[i]))
        ]
        for i in range(len(data))
    ]
    return min_column, data

def make_zero_data(data):
    zero_data = [[0 for j in range(len(data))] for i in range(len(data))]
    for i in range(len(data)):
        for j in range(len(data[i])):
            if data[i][j] == 0:
                zero_data[i][j] = get_min_column_zero(data, i, j)+get_min_row_zero(data, i, j)
    return zero_data

def find_max_zero(zero_data):
    mx = max([max(zero_data[i]) for i in range(len(zero_data))])
    for i, row in enumerate(zero_data):
        for j, val in enumerate(row):
            if val == mx:
                return [i, j]
            
def make_matrix_reduction(data, zero_id):
    return [
        [
            0 if (i == zero_id[0] or j == zero_id[1] or (i, j) == (zero_id[1], zero_id[0]))
            else data[i][j]
            for j in range(len(data[i]))
        ]
        for i in range(len(data))
    ]

if __name__=="__main__":
    data = get_data()
    main_data = copy.deepcopy(data)
    algorithm_littla(data)

