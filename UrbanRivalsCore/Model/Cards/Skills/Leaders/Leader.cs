namespace UrbanRivalsCore.Model.Cards.Skills.Leaders
{
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
