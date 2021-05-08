using CHTG.Exceptions.Utilities;
using System;
using System.IO;

namespace CHTG.Utilities
{
    public class CSVParser
    {
        public string ErrorMessage { get; set; }

        public int[][] ParseCSVToIncidencyMatrix(string csv)
        {
            var lines = File.ReadAllLines(csv);

            int[][] matrix = new int[][] { };

            try
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    var elements = lines[i].Split(',');

                    ValidateRow(lines, elements);

                    for (int j = 0; j < elements.Length; j++)
                    {
                        matrix[i][j] = Convert.ToInt32(elements[j]);
                    }
                }

                ValidateDigraf(matrix);
                ValidateOrientation(matrix);
            }
            catch(Exception ex)
            {
                ErrorMessage = ex.Message;
                return null;
            }

            return matrix;
        }

        private void ValidateRow(string[] lines, string[] elements)
        {
            if (lines.Length != elements.Length)
            {
                throw new InvalidInputFileException("Podana macierz nie jest kwadratowa.");
            }
        }

        private void ValidateDigraf(int[][] matrix)
        {
            for (int i = 0; i < matrix[0].Length; i++)
            {
                if (matrix[i][i] != 0)
                {
                    throw new InvalidInputFileException("Podany graf posiada wierzchołki które mają krawędzie do siebie.");
                }
            }

            for (int i = 0; i < matrix[0].Length; i++)
            {
                for (int j = 0; j < matrix[0].Length; j++)
                {
                    if (matrix[i][j] != 0 && matrix[i][j] != 1)
                    {
                        throw new InvalidInputFileException("Podany graf posiada wierzchołki pomiędzy którymi jest więcej niż jedna droga.");
                    }
                }
            }

            if (matrix.Length == 0)
            {
                throw new InvalidInputFileException("Podany plik jest pusty");
            }
        }

        private void ValidateOrientation(int[][] matrix)
        {
            for (int i = 0; i < matrix[0].Length; i++)
            {
                for (int j = 0; j < matrix[0].Length; j++)
                {
                    if (matrix[i][j] == 1 && matrix[j][i] == 1)
                    {
                        throw new InvalidInputFileException("Podany graf nie jest zorientowany.");
                    }
                }
            }
        }
    }
}
