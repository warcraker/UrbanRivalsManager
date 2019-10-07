namespace Warcraker.UrbanRivals.Core.Model.Cards
{
    public class LevelStats
    {
        public int Power { get; private set; }
        public int Damage { get; private set; }

        internal LevelStats(int power, int damage)
        {
            this.Power = power;
            this.Damage = damage;
        }
    }
}
