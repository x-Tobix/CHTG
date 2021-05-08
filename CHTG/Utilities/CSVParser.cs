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
    }
}
