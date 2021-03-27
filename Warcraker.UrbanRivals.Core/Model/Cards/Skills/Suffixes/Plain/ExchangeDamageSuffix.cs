using Warcraker.UrbanRivals.Localization;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills.Suffixes.Plain
{
    public class ExchangeDamageSuffix : Suffix
    {
        public ExchangeDamageSuffix() : base(-1, -1) { }

        public override string LocalizationKey
        {
            get
            {
                return LocalizationKeys.Skills.Suffixes.Plain.EXCHANGE_DAMAGE;
            }
        }
    }
}
