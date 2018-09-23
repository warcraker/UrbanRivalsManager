using System;

namespace UrbanRivalsCore.Model
{
    /// <summary>
    /// A specific level of a <see cref="CardBase"/>. Contains the numerical values that change between levels.
    /// </summary>
    public class CardLevel
    {
        /// <summary>
        /// Gets the level.
        /// </summary>
        public int Level { get; private set; }

        /// <summary>
        /// Gets the power.
        /// </summary>
        public int Power { get; private set; }

        /// <summary>
        /// Gets the damage.
        /// </summary>
        public int Damage { get; private set; }

        // Constructors

        private CardLevel() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CardLevel"/> class. 
        /// </summary>
        /// <param name="level">Level.</param>
        /// <param name="power">Power.</param>
        /// <param name="damage">Damage.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="level"/> must be between 1 and 5 inclusive</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="power"/> must be greater than 0</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="damage"/> must be greater than 0</exception>
        public CardLevel(int level, int power, int damage)
        {
            if (level < 1 || level > 5)
                throw new ArgumentOutOfRangeException(nameof(level), level, "Must be between 1 and 5 inclusive");
            if (power <= 0)
                throw new ArgumentOutOfRangeException(nameof(power), power, "Must be greater than 0");
            if (damage <= 0)
                throw new ArgumentOutOfRangeException(nameof(damage), damage, "Must be greater than 0");

            Level = level;
            Power = power;
            Damage = damage;
        }

        // Functions

        /// <summary>
        /// Returns the string representation of the instance.
        /// </summary>
        /// <returns>Level* Power/Damage</returns>
        public override string ToString()
        {
            return $"{Level}* {Power}/{Damage}";
        }
    }
}