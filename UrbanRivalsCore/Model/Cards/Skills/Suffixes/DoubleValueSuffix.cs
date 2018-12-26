using System;
using UrbanRivalsUtils;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes
{
    public abstract class DoubleValueSuffix : SingleValueSuffix
    {
        protected readonly int y;

        public DoubleValueSuffix(int x, int y) : base(x)
        {
            AssertArgument.checkIntegerRange(y >= 0, "Must be positive", y, nameof(y));

            this.y = y;
        }

        protected override string getTextRepresentation(string stringFormat)
        {
            return String.Format(stringFormat, this.x, this.y);
        }
    }
}
