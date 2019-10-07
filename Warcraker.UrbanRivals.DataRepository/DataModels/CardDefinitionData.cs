using System;
using System.Collections.Generic;
using System.Text;

namespace Warcraker.UrbanRivals.DataRepository.DataModels
{
    public class CardDefinitionData
    {
        public int Hash { get; set; }

        public int GameId { get; set; }
        public int ClanGameId { get; set; }
        public string Name { get; set; }
        public int InitialLevel { get; set; }
        public int AbilityUnlockLevel { get; set; }
        public int[] PowerPerLevel { get; set; }
        public int[] DamagePerLevel { get; set; }
        public int Rarity { get; set; }
    }
}
