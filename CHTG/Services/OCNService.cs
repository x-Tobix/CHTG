using CHTG.Services.Interfaces;
using CHTG.Utilities;
using System;

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
            int[][] matrix = _csvParser.ParseCSVToIncidencyMatrix(incidencyMatrixPath);

            switch (algorithm)
            {
                case 1:
                    return 1;
                case 2:
                    return 2;
                case 3:
                    return 3;
                default:
                    break;
            }

            return 0;
        }
    }
}
