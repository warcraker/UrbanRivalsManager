using System;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills
{
    public abstract class Suffix
    {
        public int X { get; }
        public int Y { get; }

        public Suffix(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
