using System;
using System.Collections.Generic;
using System.Text;
using Warcraker.UrbanRivals.Core.Model.Cards.Skills;

namespace Warcraker.UrbanRivals.Core.Model.Cards
{
    public class Clan
    {
        public int GameId { get; private set; }
        public string Name { get; set; }
        public Skill Bonus { get; set; }

        public Clan(int gameId, string name, Skill bonus)
        {
            this.GameId = gameId;
            this.Name = name;
            this.Bonus = bonus;
        }

        public bool Equals(Clan other)
        {
            if (other == null)
                return false;

            return this.GameId == other.GameId;
        }
    }
}
