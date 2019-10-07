using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Warcraker.UrbanRivals.DataRepository.DataModels
{
    public class BlobCycleData
    {
        [PrimaryKey]
        public int Hash { get; set; }

        public int[] AbilityHashes { get; set; }
        public int[] CardDefinitionHashes { get; set; }
        public int[] ClanHashes { get; set; }
    }
}
