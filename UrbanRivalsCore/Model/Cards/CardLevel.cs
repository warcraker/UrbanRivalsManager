using UrbanRivalsUtils;

namespace UrbanRivalsCore.Model
{
    public class CardLevel
    {
        public readonly int level;
        public readonly int power;
        public readonly int damage;

        public CardLevel(int level, int power, int damage)
        {
            AssertArgument.checkIntegerRange(level >= 1 && level <= 5, "Must be between 1 and 5 inclusive", level, nameof(level));
            AssertArgument.checkIntegerRange(power > 0, "Must be greater than 0", power, nameof(power));
            AssertArgument.checkIntegerRange(damage > 0, "Must be greater than 0", damage, nameof(damage));

            this.level = level;
            this.power = power;
            this.damage = damage;
        }

        public override string ToString()
        {
            return $"{level}* {power}/{damage}";
        }
    }
}