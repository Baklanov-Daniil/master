using System;
using System.Collections.Generic;
using System.IO;

namespace rgr
{
    class Program
    {
        static int[,] get_data(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            int vertex_count = int.Parse(lines[0]);
            int edge_count = int.Parse(lines[1]);
            int[,] data = new int[vertex_count, vertex_count];
            for (int line = 2; line < 2 + edge_count; line++)
            {
                string[] parts = lines[line].Split(' ');
                int vertex1 = int.Parse(parts[0]) - 1;
                int vertex2 = int.Parse(parts[1]) - 1;
                int weight = int.Parse(parts[2]);

                data[vertex1, vertex2] = weight;
                data[vertex2, vertex1] = weight;
            }
            return data;
        }
        static List<int> get_path(int[] vertex_before, int end)
        {
            List<int> path = new List<int>();
            if (vertex_before[end] == -1) return path;
            for (int towards = end; towards != -1; towards = vertex_before[towards]) path.Add(towards+1);
            path.Reverse();
            return path;
        }

        static void Main(string[] args)
        {
            int[,] data = get_data("data.txt");
            int start = 1;
            int end = 5;
            int vertex_count = data.GetLength(0);
            int[] distances = new int[vertex_count];
            int[] vertex_before = new int[vertex_count];
            bool[] visited_vertex = new bool[vertex_count];
            for (int vertex = 0; vertex<vertex_count; vertex++) vertex_before[vertex] = -1;
            for (int vertex = 0; vertex<vertex_count; vertex++) distances[vertex] = (vertex == start-1) ? 0 : int.MaxValue;
            for (int step = 0; step<vertex_count-1; step++){
                int closed_vertex = -1;
                int min_distance = int.MaxValue;
                for (int vertex = 0; vertex<vertex_count; vertex++)
                {
                    if (!visited_vertex[vertex] && distances[vertex]<min_distance)
                    {
                        min_distance = distances[vertex];
                        closed_vertex = vertex;
                    }
                }
                if (closed_vertex == -1) break;
                visited_vertex[closed_vertex] = true;
                for (int neighbour_vertex =  0; neighbour_vertex<vertex_count; neighbour_vertex++)
                {
                    int edge_weight = data[closed_vertex, neighbour_vertex];
                    if (edge_weight == 0 || visited_vertex[neighbour_vertex]) continue;
                    if (distances[closed_vertex] != int.MaxValue && 
                        distances[closed_vertex]+edge_weight<distances[neighbour_vertex]) 
                        {
                            distances[neighbour_vertex] = distances[closed_vertex]+edge_weight;
                            vertex_before[neighbour_vertex] = closed_vertex;
                        }
                }
            }
            List<int> path = get_path(vertex_before, end-1);
            if (path.Count == 0)
            {
                Console.WriteLine($"Пути от вершины {start} к {end} не существует");
            }
            else 
            {
                Console.WriteLine($"Пути от вершины {start} к {end}: {string.Join(" → ", path)} = {distances[end-1]}");
            }
        }
    }
}