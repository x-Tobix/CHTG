using CHTG.Services.Interfaces;
using CHTG.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CHTG.Services
{
    public class OCNService : ISolvable
    {
        private readonly CSVParser _csvParser;
        public OCNService(CSVParser csvParser)
        {
            _csvParser = csvParser;
        }

        public int FindOrientedChromaticNumber(int algorithm, string incidencyMatrixPath)
        {
            int[,] matrix = _csvParser.ParseCSVToIncidencyMatrix(incidencyMatrixPath);

            if (matrix.Length == 1)
            {
                return 1;
            }

            switch (algorithm)
            {
                case 1:
                    return Greedy(matrix);
                case 2:
                    return Gradual(matrix);
                case 3:
                    return Neighbourhood(matrix);
                default:
                    break;
            }

            return 0;
        }

        private int Neighbourhood(int[,] matrix)
        {
            int[] graph = new int[(int)Math.Sqrt(matrix.Length)];
            int chromaticNumber = 2;
            int counter = 0;

            try
            {
                while (true)
                {
                    if (counter != 0)
                    {
                        if (ValidateColoring(graph, matrix))
                        {
                            return chromaticNumber;
                        }
                    }

                    while (true)
                    {
                        int i = 0;
                        bool breakLoop = false;

                        while (true)
                        {
                            graph[i]++;
                            if (graph[i] == chromaticNumber - 1)
                            {
                                counter++;
                            }

                            if (graph[i] < chromaticNumber)
                            {
                                breakLoop = true;
                                break;
                            }

                            graph[i] = 0;
                            counter--;
                            i++;

                            if(i == graph.Length)
                            {
                                chromaticNumber++;
                                break;
                            }
                        }

                        if (breakLoop)
                        {
                            break;
                        }
                    }
                }

                throw new Exception();
            }
            catch(Exception)
            {
                Console.WriteLine("Nieoczekiwany błąd algorytmu sąsiedzkiego. Obliczenia zostają przerwane.");
                return 0;
            }
        }

        private int Gradual(int[,] matrix)
        {
            int[] graph = new int[(int)Math.Sqrt(matrix.Length)];

            //First item in tuple is vertex number, second is it's degree
            (int,int)[] degrees = new (int,int)[(int)Math.Sqrt(matrix.Length)];
            for (int i = 0; i < Math.Sqrt(matrix.Length); i++)
            {
                for (int j = 0; j < Math.Sqrt(matrix.Length); j++)
                {
                    degrees[i] = (degrees[i].Item1 + matrix[i, j] + matrix[j,i], i);
                }
            }

            degrees = degrees.OrderByDescending(x => x.Item1).ToArray();

            foreach (var degree in degrees)
            {
                graph[degree.Item2] = ColorVertex(degree.Item2, matrix, graph);
            }

            return graph.OrderByDescending(x => x).First();
        }

        private int ColorVertex(int vertex, int[,] matrix, int[] graph)
        {
            int color = 0;
            int currentColor = 0;

            while(color == 0)
            {
                currentColor++;

                if (ValidateVertexColoring(matrix, graph, vertex, currentColor))
                {
                    color = currentColor;
                }
            }

            return color;
        }

        private bool ValidateVertexColoring(int[,] matrix, int[] graph, int vertex, int color)
        {
            for (int i = 0; i < graph.Length; i++)
            {
                if (matrix[i, vertex] == 1 || matrix[vertex, i] == 1)
                {
                    if (graph[i] == color)
                    {
                        return false;
                    }
                }
            }

            List<(int, int)> edges = new List<(int, int)>();

            for (int i = 0; i < graph.Length; i++)
            {
                for (int j = 0; j < graph.Length; j++)
                {
                    if (matrix[i, j] == 1)
                    {
                        edges.Add((i, j));
                    }
                }
            }

            foreach (var edge1 in edges.Where(x => (x.Item1 == vertex && graph[x.Item2] != 0) || (x.Item2 == vertex && graph[x.Item1] != 0)))
            {
                foreach (var edge2 in edges.Where(x => x != edge1 && ((graph[x.Item1] != 0 && graph[x.Item2] != 0) || (x.Item1 == vertex && graph[x.Item2] != 0 || (x.Item2 == vertex && graph[x.Item1] != 0)))))
                {
                    if (graph[edge2.Item1] == 0 || graph[edge2.Item2] == 0)
                    {
                        if ((graph[edge1.Item1], graph[edge1.Item2]) == (graph[edge2.Item2], graph[edge2.Item1]))
                        {
                            return false;
                        }
                    }
                    else if (edge1.Item1 == vertex)
                    {
                        if ((graph[edge2.Item2], graph[edge2.Item1]) == (color, graph[edge1.Item2]))
                        {
                            return false;
                        }
                    }
                    else if (edge1.Item2 == vertex)
                    {
                        if ((graph[edge2.Item2], graph[edge2.Item1]) == (graph[edge1.Item1], color))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private int Greedy(int[,] matrix)
        {
            int chromaticNumber = 0;
            int[] graph = new int[(int)Math.Sqrt(matrix.Length)];
            for (int i = 0; i < graph.Length; i++)
            {
                graph[i] = 1;
            }

            int currentChromaticNumber = 1;
            do
            {
                currentChromaticNumber++;
                while (chromaticNumber == 0 && graph.Sum() < graph.Length * currentChromaticNumber)
                {
                    graph.AddMod(currentChromaticNumber);

                    if(ValidateColoring(graph, matrix))
                    {
                        chromaticNumber = currentChromaticNumber;
                        break;
                    }
                }
            }
            while (chromaticNumber == 0);

            return chromaticNumber;
        }

        private bool ValidateColoring(int[] graph, int[,] matrix)
        {
            for (int i = 0; i < graph.Length; i++)
            {
                for (int j = 0; j < graph.Length; j++)
                {
                    if (matrix[i,j] == 1)
                    {
                        if (graph[i] == graph[j])
                        {
                            return false;
                        }
                    }
                }
            }

            List<(int, int)> edges = new List<(int, int)>();

            for (int i = 0; i < graph.Length; i++)
            {
                for (int j = 0; j < graph.Length; j++)
                {
                    if (matrix[i,j] == 1)
                    {
                        edges.Add((i, j));
                    }
                }
            }

            foreach (var edge1 in edges)
            {
                foreach (var edge2 in edges)
                {
                    if ((graph[edge2.Item1], graph[edge2.Item2]) == (graph[edge1.Item2], graph[edge1.Item1]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
