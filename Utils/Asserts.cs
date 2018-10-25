using System;

namespace UrbanRivalsUtils
{
    public static class Asserts
    {
        private class PrvAssertException : Exception
        {
            public PrvAssertException(string message)
                : base($"Failed assertion: {message}")
            {
                ;
            }
        }

        public static void check(bool condition, string message)
        {
            if (condition == false)
                throw new PrvAssertException(message);
        }

        public static void fail(string message)
        {
            throw new PrvAssertException(message);
        }
    }

    public static class GlobalRandom
    {
        private static Random r = new Random();

        public static int Next(int max)
        {
            return r.Next(max);
        }

        public static int Next(int min, int max)
        {
            return r.Next(min, max);
        }
    }

    public static class AssertArgument
    {
        public static void fail(string message, string paramName)
        {
            throw new ArgumentException(paramName, message);
        }

        public static void check(bool condition, string message, string paramName)
        {
            if (condition == false)
            {
                throw new ArgumentException(paramName, message);
            }
        }

        public static void checkIntegerRange(bool condition, string message, int paramValue, string paramName)
        {
            if (condition == false)
            {
                throw new ArgumentOutOfRangeException(paramName, paramValue, message);
            }
        }

        public static void isNotNull<T>(T paramValue, string paramName)
        {
            if (paramValue == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        public static void stringIsFilled(string paramValue, string paramName)
        {
            if (String.IsNullOrWhiteSpace(paramValue) == true)
            {
                throw new ArgumentNullException("The string cannot be null nor whitespace", paramName);
            }
        }
    }

}
