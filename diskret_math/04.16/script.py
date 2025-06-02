import math
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

def show_matr(data):
    copy_data = copy.deepcopy(data)
    for i in range(len(copy_data)):
        for j in range(len(copy_data)):
            if copy_data[i][j] == float("inf"): copy_data[i][j]="∞"
            print(copy_data[i][j], end=" ")
        print()
    print()

def work_with_matrix(matrix):
    n = len(matrix)
    cost = 0
    new_matrix = copy.deepcopy(matrix)
    
    for i in range(n):
        finite_values = [val for val in new_matrix[i] if not math.isinf(val)]
        if not finite_values:
            continue
            
        min_row = min(finite_values)
        if min_row > 0:
            for j in range(n):
                if not math.isinf(new_matrix[i][j]):
                    new_matrix[i][j] -= min_row
            cost += min_row
    
    for j in range(n):
        min_col = math.inf
        for i in range(n):
            if not math.isinf(new_matrix[i][j]) and new_matrix[i][j] < min_col:
                min_col = new_matrix[i][j]
        
        if math.isinf(min_col) or min_col == 0:
            continue
            
        for i in range(n):
            if not math.isinf(new_matrix[i][j]):
                new_matrix[i][j] -= min_col
        cost += min_col
    
    print(cost)
    
    return new_matrix, cost

def find_zero_cf(matrix):
    n = len(matrix)
    max_cf = -math.inf
    x, y = -1, -1
    
    for i in range(n):
        for j in range(n):
            if matrix[i][j] == 0:
                row_min = math.inf
                for k in range(n):
                    if k != j and not math.isinf(matrix[i][k]):
                        row_min = min(row_min, matrix[i][k])
                
                col_min = math.inf
                for k in range(n):
                    if k != i and not math.isinf(matrix[k][j]):
                        col_min = min(col_min, matrix[k][j])
                
                cf = (row_min if not math.isinf(row_min) else 0) + \
                         (col_min if not math.isinf(col_min) else 0)
                
                if cf > max_cf:
                    max_cf = cf
                    x, y = i, j
                print( cf, i, j)
    
    return x, y, max_cf

def little_tsp(matrix):
    n = len(matrix)
    best_cost = math.inf
    stack = []
    
    init_matrix, init_bound = work_with_matrix(matrix)
    stack.append({'mat': init_matrix, 'b': init_bound, 'level': 0})
    
    while stack:
        current = stack.pop()
        mat, b, level = current['mat'], current['b'], current['level']
        
        if level == n - 1:
            best_cost = min(best_cost, b)
            continue
        
        i, j, cf = find_zero_cf(mat)
        
        if i == -1 or j == -1:
            continue
        
        include_matrix = copy.deepcopy(mat)
        current_n = len(include_matrix)
        
        for k in range(current_n):
            include_matrix[i][k] = math.inf
            include_matrix[k][j] = math.inf
        
        include_matrix[j][i] = math.inf

        print(i+1, j+1, ":", j+1, i+1)
        
        reduced1, added_cost1 = work_with_matrix(include_matrix)
        new_bound1 = b + added_cost1
        
        if new_bound1 < best_cost:
            stack.append({'mat': reduced1, 'b': new_bound1, 'level': level + 1})
        show_matr(include_matrix)
    
    return best_cost

# Чтение матрицы из файла
if __name__ == "__main__":
    matrix = get_data()
    result = little_tsp(matrix)
    print(f"\nМинимальная длина маршрута: {result}")
