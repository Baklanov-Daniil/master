def f(x):
    return (x-15)*(x-15)+5

def golden_ratio_iter(a, b, epsilon=0.01):
    t = 0.618
    L = b - a
    x1 = a + L * t
    x2 = b - L * t
    
    f1 = f(x1)
    f2 = f(x2)
    
    while L >= epsilon:
        if f1 > f2:
            b = x1
            f1 = f2
            x1 = x2
            L = b - a
            
            x2 = b - L * t
            f2 = f(x2)
        else:
            a = x2
            f2 = f1
            x2 = x1
            L = b - a
            
            x1 = a + L * t
            f1 = f(x1)
    
    return f((a + b) / 2)

a = 12
b = 20
print(golden_ratio_iter(a, b))

"""
a = 12
b = 20
L = b - a = 8
t = 0.618

Шаг 0:
a = 12.000
b = 20.000
L = 8.000

x₁ = a + L·t = 12 + 8·0.618 = 12 + 4.944 = 16.944
x₂ = b - L·t = 20 - 8·0.618 = 20 - 4.944 = 15.056

f₁ = f(16.944) = (16.944-15)² + 5 = (1.944)² + 5 = 3.779 + 5 = 8.779
f₂ = f(15.056) = (15.056-15)² + 5 = (0.056)² + 5 = 0.003 + 5 = 5.003

Шаг1:
f₁ > f₂

a = a = 12.000
b = x₁ = 16.944
f₁ = f₂ = 5.003
x₁ = x₂ = 15.056
L = b - a = 16.944 - 12.000 = 4.944

x₂ = b - L·t = 16.944 - 4.944·0.618 = 16.944 - 3.055 = 13.889  
f₂ = f(13.889) = (13.889-15)² + 5 = (-1.111)² + 5 = 1.234 + 5 = 6.234

Шаг 2:
f₁ < f₂

a = x₂ = 13.889
b = b = 16.944
f₂ = f₁ = 5.003
x₂ = x₁ = 15.056
L = b - a = 16.944 - 13.889 = 3.055

x₁ = a + L·t = 13.889 + 3.055·0.618 = 13.889 + 1.888 = 15.777
f₁ = f(15.777) = (15.777-15)² + 5 = (0.777)² + 5 = 0.604 + 5= 5.604

O(log(t, (b-a)/epsilon))
"""

"""
Дихотомия

import math

print("Метод Дихотомии, исходная функция (x - 15)^2 + 5")
print("Введите начало интервала a: ")
a = int(input())
print("Введите конец интервала b: ")
b = int(input())

epsilon = 0.01

func = lambda x: ((x - 15)**2) + 5

count = 0
while (b - a) > (2 * epsilon):
    count += 1
    print(f"\n{count} итерация (a = {a}, b = {b}):\n")
    c = (a + b) / 2
    print(f"c = {c}")
    x1 = c - (epsilon / 2)
    print(f"x1 = {x1}")
    x2 = c + (epsilon / 2)
    print(f"x2 = {x2}")
    f1 = func(x1)
    print(f"f(x1) = {f1}")
    f2 = func(x2)
    print(f"f(x2) = {f2}")
    print(f"l = {b - a}")
    if f1 > f2:
        a = x1
    else:
        b = x2

print(f"\nРезультат: x* = {(a + b) / 2} f* = {func((a + b) / 2)}")
"""

"""
Фибоначчи

import math

print("Метод Фибоначчи, исходная функция (x - 15)^2 + 5")
print("Введите начало интервала a: ")
a = int(input())
print("Введите конец интервала b: ")
b = int(input())

epsilon = 0.01

func = lambda x: ((x - 15)**2) + 5

def fibonacci_list():
    res = [0, 1]
    count1 = 0
    count2 = 1
    fib = 1
    while not(fib >= ((b - a) / epsilon)):
        fib = res[count1] + res[count2]
        res.append(fib)
        count1 += 1
        count2 += 1
    return res

fibonacci_list = fibonacci_list()
print(fibonacci_list, len(fibonacci_list)) # [0, 1, 1, 2, 3, 5, 8, 13, 21, 34, 55, 89, 144, 233, 377, 610, 987, 1597] 18
l = b - a
count = 1

x1 = a + (fibonacci_list[len(fibonacci_list) - 2] / fibonacci_list[len(fibonacci_list) - 1]) * l
x2 = b - (fibonacci_list[len(fibonacci_list) - 2] / fibonacci_list[len(fibonacci_list) - 1]) * l
f1 = func(x1)
f2 = func(x2)
print("Итерация 0:")
print(f"a = {a}, b = {b}, l = {l}, x1 = {x1}, x2 = {x2}, f1 = {f1}, f2 = {f2}")

while (len(fibonacci_list) > 2):
    print(f"Итерация {count}:")
    if (f1 > f2):
        b = x1
        f1 = f2
        x1 = x2
        l = b - a
        x2 = a + (b - x1)
        f2 = func(x2)
        print(f"a = {a}, b = {b}, l = {l}, x1 = {x1}, x2 = {x2}, f1 = {f1}, f2 = {f2}")
    else:
        a = x2
        f2 = f1
        x2 = x1
        l = b - a
        x1 = b - (x2 - a)
        f1 = func(x1)
        print(f"a = {a}, b = {b}, l = {l}, x1 = {x1}, x2 = {x2}, f1 = {f1}, f2 = {f2}")
    del fibonacci_list[-1]
    count += 1
print(f"\nx* = {(a + b) / 2}\nРезультат f(x*) = {func((a + b) / 2)}")
"""