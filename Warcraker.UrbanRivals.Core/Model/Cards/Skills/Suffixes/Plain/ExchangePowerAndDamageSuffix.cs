using Warcraker.UrbanRivals.Localization;

namespace Warcraker.UrbanRivals.Core.Model.Cards.Skills.Suffixes.Plain
{
    public class ExchangePowerAndDamageSuffix : Suffix
    {
        public ExchangePowerAndDamageSuffix() : base(-1, -1) { }

        public override string LocalizationKey
        {
            get
            {
                return LocalizationKeys.Skills.Suffixes.Plain.EXCHANGE_POWER_AND_DAMAGE;
            }
        }
    }
}
