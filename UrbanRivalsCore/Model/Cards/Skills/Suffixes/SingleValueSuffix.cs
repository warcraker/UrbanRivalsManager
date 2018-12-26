﻿using UrbanRivalsUtils;

namespace UrbanRivalsCore.Model.Cards.Skills.Suffixes
{
    public abstract class SingleValueSuffix : Suffix
    {
        protected readonly int x;

        protected SingleValueSuffix(int x)
            : base()
        {
            AssertArgument.checkIntegerRange(x > 0, "Must be greater than 0", x, nameof(x));

            this.x = x;
        }
    }
}
