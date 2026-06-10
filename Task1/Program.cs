using System.Text;

namespace Task1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(CountFutureValues(initialDeposit: 1000, years: 3, interestRate: 10));
        }

        /// <summary>
        /// Формирует и возвращает строку с расчетом сложных процентов по годам.
        /// </summary>
        /// <param name="initialDeposit">Начальный вклад.</param>
        /// <param name="years">Количество лет.</param>
        /// <param name="interestRate">Годовая процентная ставка.</param>
        /// <returns>Строка, где для каждого года расчета указана сумма накоплений.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static string CountFutureValues(decimal initialDeposit, int years, decimal interestRate)
        {
            if (initialDeposit <= 0)
            {
                throw new ArgumentOutOfRangeException("Начальный вклад должен быть положительным!");
            }
            if (years <= 0)
            {
                throw new ArgumentOutOfRangeException("Количество лет должно быть положительным!");
            }
            if (interestRate <= 0)
            {
                throw new ArgumentOutOfRangeException("Годовая процентная ставка должна быть положительной!");
            }

            var result = new StringBuilder();
            var growthFactor = interestRate / 100m + 1m;

            for (int i = 0; i < years; i++)
            {
                initialDeposit *= growthFactor;
                result.Append($"Год {i + 1}: {initialDeposit:F2} руб.\n");
            }

            return result.ToString();
        }
    }
}
