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
                int pos = level - InitialLevel;
                return stats[pos];
            }
        }

        private LevelStats[] stats;

        public CardStats(int initialLevel, int[] powers, int[] damages)
        {
            int statsLength = powers.Length;
            this.InitialLevel = initialLevel;
            this.MaxLevel = initialLevel + statsLength - 1;

            this.stats = new LevelStats[statsLength];
            for (int i = 0; i < statsLength; i++)
            {
                stats[i] = new LevelStats(powers[i], damages[i]);
            }
        }
    }
}
