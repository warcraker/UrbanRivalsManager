using System;
using UrbanRivalsUtils;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes.SingleValue
{
    public abstract class SingleValueSuffix : Suffix
    {
        protected readonly int x;

        protected SingleValueSuffix(int x) : base()
        {
            AssertArgument.CheckIntegerRange(x > 0, "Must be greater than 0", x, nameof(x));

            this.x = x;
        }

        protected virtual string getTextRepresentation(string stringFormat)
        {
            return String.Format(stringFormat, this.x);
        }
    }
}
