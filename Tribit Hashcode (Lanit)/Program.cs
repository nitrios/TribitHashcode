using System;
using System.Collections.Generic;

namespace Tribit
{
    class Program
    {
        private const int CellSize = 4;
        private static readonly IDictionary<string, string> Rules = new Dictionary<string, string>
        {
            {"0000", "0000"},
            {"0001", "1000"},
            {"0010", "0001"},
            {"0011", "0010"},
            {"0100", "0000"},
            {"0101", "0010"},
            {"0110", "1011"},
            {"0111", "1011"},
            {"1000", "0100"},
            {"1001", "0101"},
            {"1010", "0111"},
            {"1011", "1111"},
            {"1100", "1101"},
            {"1101", "1110"},
            {"1110", "0111"},
            {"1111", "1111"}
        };

        private static char[][] GetPyramid(string input)
        {
            var lenght = input.Length;
            var pow = 0;
            var t = 1;
            while (t < lenght)
            {
                pow++;
                t = t * CellSize;
            }

            if (lenght != 1 && (int) Math.Pow(CellSize, pow) != lenght)
                throw new ArgumentException(string.Format("The number of cells '{0}' is not a power at {1}!", input,
                    CellSize));

            var rowCount = (int) Math.Pow(2, pow);
            var pyramid = new char[rowCount][];

            var position = 0;
            for (var i = rowCount - 1; i >= 0; i--)
            {
                var symbolsCount = (i + 1) * 2 - 1;

                pyramid[i] = new char[symbolsCount];
                for (var j = symbolsCount - 1; j >= 0; j--)
                {
                    pyramid[i][j] = input[position];
                    position++;
                }
            }

            return pyramid;
        }

        private static char[][] Convert(char[][] pyramid, int length)
        {
            var reduced = false;
            var canReduce = true;

            string[] cells = {};
            int[,] coords = {};

            while (canReduce)
            {
                var cellPosition = 0;
                var cellCount = length / CellSize;

                coords = new int[cellCount, 4];
                cells = new string[cellCount];
                for (var i = 0; i < cellCount; i++)
                    cells[i] = string.Empty;

                for (var i = 0; i < pyramid.Length / 2; i++)
                {
                    var currentRowCellCount = i * 2 + 1;
                    for (var j = 0; j < currentRowCellCount; j++)
                    {
                        var pRow = i * 2;
                        var pSecondRow = pRow + 1;

                        var pPosition = j * 2;
                        var pSecondPosition = pPosition;

                        if (j % 2 == 1)
                        {
                            pRow++;
                            pSecondRow--;

                            pPosition++;
                            pSecondPosition--;
                        }

                        coords[cellPosition, 0] = pRow;
                        coords[cellPosition, 1] = pPosition;
                        coords[cellPosition, 2] = pSecondRow;
                        coords[cellPosition, 3] = pSecondPosition;

                        cells[cellPosition] = string.Format("{0}{1}{2}{3}",
                            pyramid[pSecondRow][pSecondPosition + 2],
                            pyramid[pSecondRow][pSecondPosition + 1],
                            pyramid[pSecondRow][pSecondPosition + 0],
                            pyramid[pRow][pPosition]);

                        if (cells[cellPosition] != "1111" && cells[cellPosition] != "0000")
                            canReduce = false;

                        cellPosition++;
                    }
                }

                if (canReduce)
                {
                    reduced = true;

                    var result = string.Empty;
                    for (var i = cellCount - 1; i >= 0; i--)
                        result += cells[i][0];

                    pyramid = GetPyramid(result);
                    length = result.Length;

                    if (length == 1)
                        return pyramid;
                }
            }

            if (!reduced)
                for (var i = 0; i < cells.Length; i++)
                {
                    var newString = Rules[cells[i]];
                    pyramid[coords[i, 2]][coords[i, 3] + 2] = newString[0];
                    pyramid[coords[i, 2]][coords[i, 3] + 1] = newString[1];
                    pyramid[coords[i, 2]][coords[i, 3] + 0] = newString[2];
                    pyramid[coords[i, 0]][coords[i, 1]] = newString[3];
                }

            return pyramid;
        }

        private static string PyramidToString(char[][] pyramid)
        {
            var result = string.Empty;

            for (var i = 0; i < pyramid.Length; i++)
            for (var j = 0; j < pyramid[i].Length; j++)
                result = pyramid[i][j] + result;

            return result;
        }

        private static void Process(string input)
        {
            if (input.Replace("1", "").Replace("0", "").Length != 0)
                throw new ArgumentException("Input string must cointains only '0' or '1' symbols!");

            var pyramid = GetPyramid(input);
            while (true)
            {
                input = PyramidToString(pyramid);
                Console.WriteLine(input);

                if (input.Length == 1)
                    break;

                pyramid = Convert(pyramid, input.Length);
            }

            Console.WriteLine();
        }

        private static void Main(string[] args)
        {
            Process("0100");
            Process("0001001111100110");
            Process("1100100100001111110110101010001000100001011010001100001000110100");

            Process("0000000000000000000000000000000000000000000000000000000000000000");
            Process("0001111111101111111111111111111101111111111111111111011111111111");

            Process("1110000000000001000000000000000000000000000000000000000000000000");
            Process("0000111000010000");

            if (args.Length != 0)
                Process(args[0]);

            Console.ReadKey();
        }
    }
}
