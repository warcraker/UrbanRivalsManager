using System;

namespace UrbanRivalsCore
{
    /// <summary>
    /// Centralized random number generator.
    /// </summary>
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
}