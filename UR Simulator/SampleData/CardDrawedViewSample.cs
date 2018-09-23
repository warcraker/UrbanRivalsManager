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
        CardDrawed card;

        public CardDrawedViewSample()
        {
            int baseId = 123;
            Clan clan = Clan.GetClanById(ClanId.Raptors);
            string cardName = "Tester";
            Skill ability = new Skill(SkillPrefix.Courage, SkillSuffix.DecreasePillzXMinY, 2, 3);
            int minLevel = 2;
            int maxLevel = 4;
            List<CardLevel> cardLevels = new List<CardLevel>
            {
                new CardLevel(2,2,3),
                new CardLevel(3,3,4),
                new CardLevel(4,6,5),
            };
            int abilityUnlockLevel = 3;
            CardRarity rarity = CardRarity.Legendary;
            int currentLevel = 4;
            int instanceId = 222;


            CardBase cardBase = new CardBase(baseId, cardName, clan, minLevel, maxLevel, cardLevels, ability, abilityUnlockLevel, rarity);
            CardInstance cardInstance = new CardInstance(cardBase, instanceId, currentLevel);
            card = new CardDrawed(cardInstance);
        }

        public bool IsSelected { get { return true; } }
        public bool HasBattled { get { return true; } }
        public int UsedPillz { get { return 5; } }
        public bool HasUsedFury { get { return false; } }
        public bool HasWon { get { return false; } }
        public CardDrawed CardDrawed { get { return card; } }

    }
}
