using System.Collections.Generic;
using System.Linq;

namespace ExtractInfoSaveSettlementsLog.Utils.Math
{
    public static class Statistic
    {
        public static double Average(List<double> numbers, int windowSize)
        {
            if (numbers.Count == 0)
            {
                return 0;
            }

            int index = numbers.Count > windowSize ? numbers.Count - windowSize : 0;
            int count = numbers.Count > windowSize ? windowSize : numbers.Count;

            if (count == 0 || index < 0 || index >= numbers.Count)
            {
                return 0;
            }

            List<double> subList = numbers.GetRange(index, count);

            if (subList.Count == 0)
            {
                return 0;
            }

            double avg = subList.Average();

            return avg;
        }

        public static double Median(List<double> numbers, int windowSize)
        {
            if (numbers.Count == 0)
            {
                return 0;
            }

            int index = numbers.Count > windowSize ? numbers.Count - windowSize : 0;
            int count = numbers.Count > windowSize ? windowSize : numbers.Count;

            if (count == 0 || index < 0 || index >= numbers.Count)
            {
                return 0;
            }

            List<double> subList = numbers.GetRange(index, count);

            if (subList.Count == 0)
            {
                return 0;
            }

            int numberCount = subList.Count;
            int halfIndex = subList.Count / 2;

            IOrderedEnumerable<double> sortedNumbers = subList.OrderBy(n => n);

            double median;

            if (numberCount % 2 == 0)
            {
                int half1 = halfIndex;
                int half2 = halfIndex - 1;
                median = sortedNumbers.ElementAt(half1) + sortedNumbers.ElementAt(half2);
                median = median / 2;
            }
            else
            {
                median = sortedNumbers.ElementAt(halfIndex);
            }

            return median;
        }

        public static double Rate(double number1, double number2)
        {
            double rate = number2 > 0 ? number1 / number2 : 0.0;

            return rate;
        }
    }
}
