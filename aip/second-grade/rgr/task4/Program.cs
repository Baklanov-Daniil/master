using System;
using System.Collections.Generic;
using System.IO;

namespace diskret_rgr
{
    class Program
    {
        static int[,] GetData(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            int vertexCount = int.Parse(lines[0]);
            int edge_count = int.Parse(lines[1]);
            int[,] data = new int[vertexCount, vertexCount];
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

        static void Main(string[] args)
        {
            int[,] data = GetData("data.txt");
            int start = 1;
            int end = 3;
            int vertexCount = data.GetLength(0);
            int[] distances = new int[vertexCount];
            int[] vertexBefore = new int[vertexCount];
            bool[] visited_vertex = new bool[vertexCount];
            for (int vertex = 0; vertex<vertexCount; vertex++) vertexBefore[vertex] = -1;
            for (int vertex = 0; vertex<vertexCount; vertex++) distances[vertex] = (vertex == start-1) ? 0 : int.MaxValue;
            for (int step = 0; step<vertexCount-1; step++)
            {
                int closedVertex = -1;
                int min_distance = int.MaxValue;
                for (int vertex = 0; vertex<vertexCount; vertex++)
                {
                    if (!visited_vertex[vertex] && distances[vertex]<min_distance)
                    {
                        min_distance = distances[vertex];
                        closedVertex = vertex;
                    }
                }
                if (closedVertex == -1) break;
                visited_vertex[closedVertex] = true;
                for (int neighborVertex =  0; neighborVertex<vertexCount; neighborVertex++)
                {
                    int edge_weight = data[closedVertex, neighborVertex];
                    if (edge_weight == 0 || visited_vertex[neighborVertex]) continue;
                    if (distances[closedVertex] != int.MaxValue && 
                        distances[closedVertex]+edge_weight<distances[neighborVertex]) 
                        {
                            distances[neighborVertex] = distances[closedVertex]+edge_weight;
                            vertexBefore[neighborVertex] = closedVertex;
                        }
                }
            }
            if (distances[end-1] == int.MaxValue)
            {
                Console.WriteLine(-1);
            }
            else 
            {
                Console.WriteLine(distances[end-1]);
            }
        }
    }
}