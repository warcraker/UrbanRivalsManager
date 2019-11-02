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
#if DEBUG
            if (condition == false)
            {
                throw new AssertException(message);
            }
#endif
        }

        public static void Fail(string message)
        {
#if DEBUG
            throw new AssertException(message);
#endif
        }
    }

    public static class AssertArgument
    {
        public static void Fail(string message, string paramName)
        {
#if DEBUG
            throw new ArgumentException(paramName, message);
#endif
        }

        public static void Check(bool condition, string message, string paramName)
        {
#if DEBUG
            if (condition == false)
            {
                throw new ArgumentException(paramName, message);
            }
#endif
        }

        public static void CheckIntegerRange(bool condition, string message, int paramValue, string paramName)
        {
#if DEBUG
            if (condition == false)
            {
                throw new ArgumentOutOfRangeException(paramName, paramValue, message);
            }
#endif
        }

        public static void CheckIsNotNull<T>(T paramValue, string paramName)
        {
#if DEBUG
            if (paramValue == null)
            {
                throw new ArgumentNullException(paramName);
            }
#endif
        }

        public static void StringIsFilled(string paramValue, string paramName)
        {
#if DEBUG
            if (String.IsNullOrWhiteSpace(paramValue) == true)
            {
                throw new ArgumentNullException("The string cannot be null nor whitespace", paramName);
            }
#endif
        }
    }
}
