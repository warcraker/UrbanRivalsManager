namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills.Leaders
{
    // TODO revise leader gamestrings
    public abstract class Leader
    {
        private readonly string abilityText;

        protected Leader(string abilityText)
        {
            this.abilityText = abilityText;
        }

        public bool isMatch(string abilityText)
        {
            return this.abilityText == abilityText;
        }
        public override string ToString()
        {
            return this.abilityText;
        }
    }
}
