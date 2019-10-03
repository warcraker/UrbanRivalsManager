using System;

namespace Warcraker.Utils
{
    public static class Asserts
    {
        private class AssertException : Exception
        {
            public AssertException(string message) : base($"Failed assertion: {message}")
            {
                ;
            }
        }

        public static void Check(bool condition, string message)
        {
            if (condition == false)
            {
                throw new AssertException(message);
            }
        }

        public static void Fail(string message)
        {
            throw new AssertException(message);
        }
    }

    public static class AssertArgument
    {
        public static void Fail(string message, string paramName)
        {
            throw new ArgumentException(paramName, message);
        }

        public static void Check(bool condition, string message, string paramName)
        {
            if (condition == false)
            {
                throw new ArgumentException(paramName, message);
            }
        }

        public static void CheckIntegerRange(bool condition, string message, int paramValue, string paramName)
        {
            if (condition == false)
            {
                throw new ArgumentOutOfRangeException(paramName, paramValue, message);
            }
        }

        public static void CheckIsNotNull<T>(T paramValue, string paramName)
        {
            if (paramValue == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        public static void StringIsFilled(string paramValue, string paramName)
        {
            if (String.IsNullOrWhiteSpace(paramValue) == true)
            {
                throw new ArgumentNullException("The string cannot be null nor whitespace", paramName);
            }
        }
    }
}
