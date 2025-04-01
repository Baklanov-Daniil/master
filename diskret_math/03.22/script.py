# алгоритм форда-белмана

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

def ford_bellman(data, start):
    lambda_list = [float("inf") for _ in data]
    lambda_list[start] = 0
    data_len = len(data)
    for _ in range(data_len-1):
        update = False
        for first_vertx in range(data_len):
            for second_vertex in range(data_len):
                if data[first_vertx][second_vertex]!=0 and lambda_list[first_vertx] + data[first_vertx][second_vertex] < lambda_list[second_vertex]:
                    lambda_list[second_vertex] = lambda_list[first_vertx] + data[first_vertx][second_vertex]
                    update = True
        if not update: break
    print(lambda_list)

if __name__=="__main__":
    data = get_data()
    ford_bellman(data, 0)

    