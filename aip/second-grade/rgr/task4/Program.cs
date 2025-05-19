using System;
using System.Collections.Generic;
using System.IO;

namespace dikret_rgr
{
    class Program
    {
        static int[,] GetData(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            int vertexCount = int.Parse(lines[0]);
            int edgeCount = int.Parse(lines[1]);
            int[,] data = new int[vertexCount, vertexCount];
            for (int i = 0; i < vertexCount; i++)
            {
                for (int j = 0; j < vertexCount; j++)
                {
                    data[i, j] = (i == j) ? 0 : int.MaxValue;
                }
            }
            for (int line = 2; line < 2 + edgeCount; line++)
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
        static List<int> GetPath(int[] vertexBefore, int end)
        {
            List<int> path = new List<int>();
            if (vertexBefore[end] == -1) return path;
            for (int towards = end; towards != -1; towards = vertexBefore[towards]) path.Add(towards+1);
            path.Reverse();
            return path;
        }

        static void Main(string[] args)
        {
            int[,] data = GetData("data.txt");
            int start = 1;
            int end = 5;
            int vertexCount = data.GetLength(0);
            int[] distances = new int[vertexCount];
            int[] vertexBefore = new int[vertexCount];
            bool[] visited_vertex = new bool[vertexCount];
            for (int vertex = 0; vertex<vertexCount; vertex++) vertexBefore[vertex] = -1;
            for (int vertex = 0; vertex<vertexCount; vertex++) distances[vertex] = (vertex == start-1) ? 0 : int.MaxValue;
            for (int step = 0; step<vertexCount-1; step++)
            {
                int closed_vertex = -1;
                int minDistance = int.MaxValue;
                for (int vertex = 0; vertex<vertexCount; vertex++)
                {
                    if (!visited_vertex[vertex] && distances[vertex]<minDistance)
                    {
                        minDistance = distances[vertex];
                        closed_vertex = vertex;
                    }
                }
                if (closed_vertex == -1) break;
                visited_vertex[closed_vertex] = true;
                for (int neighborVertex =  0; neighborVertex<vertexCount; neighborVertex++)
                {
                    int edge_weight = data[closed_vertex, neighborVertex];
                    if (edge_weight == 0 || visited_vertex[neighborVertex]) continue;
                    if (distances[closed_vertex] != int.MaxValue && 
                        distances[closed_vertex]+edge_weight<distances[neighborVertex]) 
                        {
                            distances[neighborVertex] = distances[closed_vertex]+edge_weight;
                            vertexBefore[neighborVertex] = closed_vertex;
                        }
                }
            }
            List<int> path = GetPath(vertexBefore, end-1);
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