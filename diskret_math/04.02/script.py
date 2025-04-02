# волновой алгоритм
import copy

matr = [
        ["0", "0", "0", "x", "0"], 
        ["0", "x", "0", "0", "0"], 
        ["0", "x", "0", "x", "0"], 
        ["0", ">", "0", "x", "<"], 
        ["0", "0", "0", "x", "0"], 
       ]

def show_matr(data):
    copy_data = copy.deepcopy(data)
    for i in range(len(copy_data)):
        for j in range(len(copy_data)):
            if copy_data[i][j] == float("inf"): copy_data[i][j]="∞"
            print(copy_data[i][j], end=" ")
        print()
    print()
        
def surround(matr):
    surround_matr = []
    matr_len = len(matr)
    for i in range(matr_len+1):
        if i == 0: surround_matr.append(["x" for _ in range(matr_len+2)])
        if i == matr_len: surround_matr.append(["x" for _ in range(matr_len+2)])
        else: 
            surround_matr.append(["x"])
            for elem in matr[i]:
                if elem in ["x", "<", ">"]: surround_matr[i+1].append(elem)
                else: surround_matr[i+1].append(float("inf"))
            surround_matr[i+1].append("x")
    return surround_matr

def find_start_end(matr):
    start = end = []
    for i in range(len(matr)):
        for j in range(len(matr)):
            if matr[i][j]==">": start = [i, j]
            elif matr[i][j]=="<": end = [i, j]
    return start, end


def get_points(matr, point):
    p1 = [point[0], point[1]-1]
    p2 = [point[0]-1, point[1]]
    p3 = [point[0], point[1]+1]
    p4 = [point[0]+1, point[1]]
    return p1, p2, p3, p4

def add_points(matr, point: list, queue: list, proccesd_point):
    p1, p2, p3, p4 = get_points(matr, point)
    for p in [p1, p2, p3, p4]:
        if matr[p[0]][p[1]]!="x" and p not in proccesd_point:
            queue.append(p)
    return queue

def set_value(matr, point):
    p1, p2, p3, p4 = get_points(matr, point)
    value_list = []
    for p in [p1, p2, p3, p4]:
        if matr[p[0]][p[1]]!="x" and matr[p[0]][p[1]]!="<":
            value_list.append(matr[p[0]][p[1]])
    matr[point[0]][point[1]] = min(value_list)+1

if __name__=="__main__":
    matr = surround(matr)
    start, end = find_start_end(matr)
    queue = [start]
    procced_points = []
    while queue:
        point = queue[0]
        if point == start:
            matr[point[0]][point[1]] = 0
        else:
            set_value(matr, point)
        add_points(matr, point, queue, procced_points)
        procced_points.append(point)
        queue.remove(point)
    if matr[end[0]][end[1]] not in [float("inf"), "<"]: print(f"Выход найден: его координаты {matr[end[0]][end[1]]}")
    else: print("Выход не достижим")
        
        
