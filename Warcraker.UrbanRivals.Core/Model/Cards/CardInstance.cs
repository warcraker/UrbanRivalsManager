using Warcraker.UrbanRivals.Core.Model.Cards.Skills;
using Warcraker.Utils;

namespace Warcraker.UrbanRivals.Core.Model.Cards
{
    public class CardInstance
    {
        public int Level { get; }
        public int Experience { get; }

        public int Id { get; }
        public string Name { get; }
        public Clan Clan { get; }

        public int Power { get; }
        public int Damage { get; }
        public Skill Ability { get; }

        public CardInstance(CardDefinition definition, int level, int experience)
        {
            // TODO asserts

            this.Level = level;
            this.Experience = experience;

            this.Id = definition.GameId;
            this.Name = definition.Name;
            this.Clan = definition.Clan;
            this.Power = definition.Stats[Level].Power;
            this.Damage = definition.Stats[Level].Damage;
            
            if (definition.Ability.Equals(NoAbility.INSTANCE))
            {
                this.Ability = NoAbility.INSTANCE;
            }
            else if (level >= definition.AbilityUnlockLevel)
            {
                this.Ability = definition.Ability;
            }
            else
            {
                switch (definition.AbilityUnlockLevel)
                {
                    case 2:
                        this.Ability = LockedSkill.UNLOCKED_AT_LEVEL_2;
                        break;
                    case 3:
                        this.Ability = LockedSkill.UNLOCKED_AT_LEVEL_3;
                        break;
                    case 4:
                        this.Ability = LockedSkill.UNLOCKED_AT_LEVEL_4;
                        break;
                    case 5:
                        this.Ability = LockedSkill.UNLOCKED_AT_LEVEL_5;
                        break;
                    default:
                        this.Ability = UnknownSkill.INSTANCE;
                        Asserts.Fail("Invalid skill");
                        break;
                }
            }
        }
    }
}
