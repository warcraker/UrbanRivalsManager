using Warcraker.UrbanRivals.Localization;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills
{
    public abstract class Suffix : ILocalizable
    {
        public int X { get; }
        public int Y { get; }
        public abstract string LocalizationKey { get; }

        public Suffix(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
