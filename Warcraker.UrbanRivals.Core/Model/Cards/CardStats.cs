using System.Collections.Generic;
using System.Linq;
using Warcraker.Utils;

namespace Warcraker.UrbanRivals.Core.Model.Cards
{
    public class CardStats
    {
        public int InitialLevel { get; private set; }
        public int MaxLevel { get; set; }
        public LevelStats this[int level] 
        {
            get
            {
                int pos = level - this.InitialLevel;
                return this.stats[pos];
            }
        }

        private readonly LevelStats[] stats;

        public CardStats(int initialLevel, IEnumerable<int> powers, IEnumerable<int> damages)
        {
            AssertArgument.CheckIntegerRange(initialLevel >= 1 && initialLevel <= 5, "must be between 1 and 5 inclusive", initialLevel, nameof(initialLevel));
            AssertStats(powers, nameof(powers));
            AssertStats(damages, nameof(damages));
            AssertArgument.Check(powers.Count() == damages.Count(), "both arguments must contain an equal number of elements", $"{nameof(powers)} & {nameof(damages)}");

            int[] powersArray = powers.ToArray();
            int[] damagesArray = damages.ToArray();

            int statsLength = powersArray.Length;
            this.InitialLevel = initialLevel;
            this.MaxLevel = initialLevel + statsLength - 1;

            this.stats = new LevelStats[statsLength];
            for (int i = 0; i < statsLength; i++)
            {
                this.stats[i] = new LevelStats(powersArray[i], damagesArray[i]);
            }
        }

        private static void AssertStats(IEnumerable<int> stats, string name)
        {
            AssertArgument.CheckIsNotNull(stats, name);
            int count = stats.Count();
            AssertArgument.CheckIntegerRange(count >= 1 && count <= 5, "must contain between 1 and 5 items inclusive", count, name);
        }
    }
}
