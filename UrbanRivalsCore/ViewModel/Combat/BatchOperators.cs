using System;
using System.Collections.Generic;
using System.Linq;

namespace UrbanRivalsCore.ViewModel
{
    internal class BatchSubtracter
    {
        private SortedList<int, int> Operations;

        public BatchSubtracter()
        {
            Operations = new SortedList<int, int>(new NotEqualIntegerComparer());
        }

        public void InsertSubstraction(int value, int min)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value), value, "Must be greater than 0");
            if (min < 0)
                throw new ArgumentOutOfRangeException(nameof(min), min, "Must be greater than or equal to 0");

            Operations.Add(min, value);
        }
        public int CalculateSubstractionsAndReset(int initialValue)
        {
            int result = initialValue;
            while (Operations.Count > 0)
            {
                int min = Operations.Keys[Operations.Count - 1];
                int value = Operations.Values[Operations.Count - 1];
                result = Math.Min(result, Math.Max(result - value, min));
                Operations.RemoveAt(Operations.Count - 1);
            }
            return result;
        }
    }
    internal class BatchAdder
    {
        private SortedList<int, int> Operations;

        public BatchAdder()
        {
            Operations = new SortedList<int, int>(new NotEqualIntegerComparer());
        }

        public void InsertAddition(int value)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value), value, "Must be greater than 0");

            Operations.Add(0, value);
        }
        public void InsertAddition(int value, int max)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value), value, "Must be greater than 0");
            if (max <= 0)
                throw new ArgumentOutOfRangeException(nameof(max), max, "Must be greater than 0");

            Operations.Add(max, value);
        }
        public int CalculateAdditionsAndReset(int initialValue)
        {
            List<int> OperationsWithoutMax = new List<int>();
            while (Operations.Count > 0)
            {
                if (Operations.Keys[0] != 0)
                    break;

                OperationsWithoutMax.Add(Operations.Values[0]);
                Operations.RemoveAt(0);
            }

            int result = initialValue;
            while (Operations.Count > 0)
            {
                int max = Operations.Keys[0];
                int value = Operations.Values[0];
                result = Math.Max(result, Math.Min(result + value, max));
                Operations.RemoveAt(0);
            }

            foreach (int value in OperationsWithoutMax)
                result += value;

            return result;
        }
    }

    internal class NotEqualIntegerComparer : IComparer<int>
    {
        public int Compare(int x, int y)
        {
            int result = x.CompareTo(y); 
            if (result == 0)
                return 1;
            return result;
        }
    }
}