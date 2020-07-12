using System.Collections.Generic;
using System.Linq;

namespace Warcraker.UrbanRivals.Core.Model.Cards
{
    public class CardStats
    {
        // TODO asserts

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
    }
}
