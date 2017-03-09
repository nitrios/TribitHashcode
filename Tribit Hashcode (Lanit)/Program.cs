using System;
using System.Collections.Generic;

namespace Tribit
{
    class Program
    {
        private const int CellSize = 4;
        private static readonly Dictionary<string, string> Rules = new Dictionary<string, string>
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

            if ((int)Math.Pow(CellSize, pow) != lenght)
                throw new ArgumentException(string.Format("The number of cells '{0}' is not a power at {1}!", input, CellSize));

            var rowCount = (int)Math.Pow(2, pow);
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

            Console.WriteLine(PiramidToString(pyramid));
            if (lenght == 1)
                return pyramid;

            var cellPosition = 0;
            var cellCount = lenght / CellSize;
            var cells = new string[cellCount];
            for (var i = 0; i < cellCount; i++)
                cells[i] = string.Empty;

            var wasConverted = false;
            for (var i = 0; i < pyramid.Length / 2; i++)
            {
                var currentRowCellCount = i * 2 + 1;
                var isUpDown = false;

                for (var j = 0; j < currentRowCellCount; j++)
                {
                    var pRow = i * 2;
                    var pSecondRow = pRow + 1;

                    var pPosition = j * 2;
                    var pSecondPosition = pPosition;

                    if (isUpDown)
                    {
                        pRow++;
                        pSecondRow--;

                        pPosition++;
                        pSecondPosition--;
                    }

                    var cellString = string.Format("{0}{1}{2}{3}",
                        pyramid[pSecondRow][pSecondPosition + 2],
                        pyramid[pSecondRow][pSecondPosition + 1],
                        pyramid[pSecondRow][pSecondPosition + 0],
                        pyramid[pRow][pPosition]);

                    if (cellString != "1111" && cellString != "0000")
                    {
                        wasConverted = true;
                        cellString = Rules[cellString];

                        pyramid[pSecondRow][pSecondPosition + 2] = cellString[0];
                        pyramid[pSecondRow][pSecondPosition + 1] = cellString[1];
                        pyramid[pSecondRow][pSecondPosition + 0] = cellString[2];
                        pyramid[pRow][pPosition] = cellString[3];
                    }

                    isUpDown = !isUpDown;

                    cells[cellPosition] = cellString;
                    cellPosition++;
                }
            }

            if (wasConverted) return pyramid;

            var result = string.Empty;

            for (var i = cellCount - 1; i >= 0; i--)
                result += cells[i][0];

            return GetPyramid(result);
        }

        private static string PiramidToString(char[][] pyramid)
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

            do
            {
                input = PiramidToString(GetPyramid(input));
            } while (input.Length != 1);

            Console.WriteLine();
        }

        private static void Main(string[] args)
        {
            Process("0100");
            Process("0001001111100110");
            Process("1100100100001111110110101010001000100001011010001100001000110100");

            if (args.Length != 0)
                Process(args[0]);
        }

    }
}
