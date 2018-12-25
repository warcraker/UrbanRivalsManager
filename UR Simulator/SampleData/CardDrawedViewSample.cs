using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UrbanRivalsCore.Model;

namespace UrbanRivalsManager.SampleData
{
    class CardDrawedViewSample : DependencyObject
    {
        private readonly CardDrawed card;

        public CardDrawedViewSample()
        {
            int baseId = 123;
            Clan clan = Clan.getClanById(ClanId.Raptors);
            string cardName = "Tester";
            OldSkill ability = new OldSkill(SkillPrefix.Courage, SkillSuffix.DecreasePillzXMinY, 2, 3);
            List<CardStats> cardStatsPerLevel = new List<CardStats>
            {
                new CardStats(2,2,3),
                new CardStats(3,3,4),
                new CardStats(4,6,5),
            };
            int abilityUnlockLevel = 3;
            CardRarity rarity = CardRarity.Legendary;
            int currentLevel = 4;
            int instanceId = 222;

            CardDefinition cardDefinition = CardDefinition.createCardWithAbility(baseId, cardName, clan, cardStatsPerLevel, rarity, ability, abilityUnlockLevel);
            CardInstance cardInstance = CardInstance.createCardInstance(cardDefinition, instanceId, currentLevel, 0);

            this.card = new CardDrawed(cardInstance);
        }

        public bool IsSelected { get { return true; } }
        public bool HasBattled { get { return true; } }
        public int UsedPillz { get { return 5; } }
        public bool HasUsedFury { get { return false; } }
        public bool HasWon { get { return false; } }
        public CardDrawed CardDrawed { get { return card; } }

    }
}
