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

            switch (algorithm)
            {
                case 1:
                    return Greedy(matrix);
                case 2:
                    return 2;
                case 3:
                    return 3;
                default:
                    break;
            }

            return 0;
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
