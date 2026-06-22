using System.Text;

namespace Task2
{
    public class Program
    {
        static void Main(string[] args)
        {
            PrintDiamond(5);
        }

        /// <summary>
        /// Выводит на экран ромб из символов X.
        /// </summary>
        /// <param name="diagonalLength">Длина диагонали ромба.</param>
        public static void PrintDiamond(int diagonalLength)
        {
            if (diagonalLength <= 0)
            {
                throw new ArgumentOutOfRangeException("Длина диагонали должна быть положительной!");
            }
            if (diagonalLength % 2 == 0)
            {
                throw new ArgumentOutOfRangeException("Длина диагонали должна быть нечетной!");
            }

            var result = new StringBuilder();

            for (var i = 0; i < diagonalLength; i++)
            {
                result.Append(' ', diagonalLength);
                result.AppendLine();

                int rowCentre = i * (diagonalLength + 2) + diagonalLength / 2;
                int offset = diagonalLength / 2 - Math.Abs(i - diagonalLength / 2);

                result[rowCentre + offset] = 'X';
                result[rowCentre - offset] = 'X';
            }

            Console.WriteLine(result);
        }
    }
}

