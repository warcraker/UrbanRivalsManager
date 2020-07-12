using System;
using System.IO;
using Tortuga.Chain;
using Tortuga.Chain.SQLite;
using Warcraker.UrbanRivals.DataRepository.DataModels;
using Warcraker.Utils;

namespace Warcraker.UrbanRivals.DataRepository
{
    public class GameDataRepository
    {
        private readonly SQLiteDataSource Connection;

        public GameDataRepository(string path)
        {
            // TODO ensure connection is safe: File exists + Test connection

            bool fileExists = File.Exists(path);

            this.Connection = new SQLiteDataSource($"Data Source={path};Database=UR;");

            this.Connection.TestConnection();

            if (!fileExists)
            {
                Initialize();
            }
        }

        public ClanData GetClanData(int clanHash)
        {
            return GetSingleItemByKey<ClanData>(clanHash);
        }
        public CardData GetCardData(int cardHash)
        {
            return GetSingleItemByKey<CardData>(cardHash);
        }
        public CycleData GetCycleData(int cycleHash)
        {
            return GetSingleItemByKey<CycleData>(cycleHash);
        }
        public SkillData GetSkillData(int skillHash)
        {
            return GetSingleItemByKey<SkillData>(skillHash);
        }
        public bool TryGetSkillHashFromTextHash(int textHash, out int skillHash)
        {
            TextToSkillData item;
            item = GetSingleItemByKey<TextToSkillData>(textHash);

            bool itemFound = item != null;
            skillHash = itemFound ? item.SkillHash : 0;

            return itemFound;
        }

        public bool SaveClanData(ClanData clanData)
        {
            return InsertSingleItem(clanData);
        }
        public bool SaveCardData(CardData cardData)
        {
            return InsertSingleItem(cardData);
        }
        public bool SaveCycleBlobData(CycleData blobData)
        {
            return InsertSingleItem(blobData);
        }
        public void SaveSkillData(SkillData skillData, int skillTextHash)
        {
            AssertArgument.CheckIsNotNull(skillData, $"Cannot insert null item [{typeof(SkillData).Name}]");

            using (SQLiteTransactionalDataSource transaction = this.Connection.BeginTransaction())
            {
                TextToSkillData textToSkillData = new TextToSkillData
                {
                    TextHash = skillTextHash,
                    SkillHash = skillData.Hash,
                };

                transaction.Upsert(skillData).Execute();
                transaction.Upsert(textToSkillData).Execute();
                transaction.Commit();
            }
        }

        private void Initialize()
        {
            string[] createTableScripts = new string[] {
@"CREATE TABLE ""CardData"" (
    ""Hash"" integer primary key not null ,
    ""GameId"" integer ,
    ""ClanGameId"" integer ,
    ""Name"" varchar ,
    ""InitialLevel"" integer ,
    ""AbilityUnlockLevel"" integer ,
    ""PowerPerLevel"" varchar ,
    ""DamagePerLevel"" varchar ,
    ""Rarity"" integer )"
, @"CREATE TABLE ""ClanData"" (
    ""Hash"" integer primary key not null ,
    ""BonusHash"" integer ,
    ""GameId"" integer ,
    ""Name"" varchar )"
, @"CREATE TABLE ""CycleData"" (
    ""Hash"" integer primary key not null ,
    ""AbilityHashes"" varchar ,
    ""CardHashes"" varchar ,
    ""ClanHashes"" varchar )"
, @"CREATE TABLE ""SkillData"" (
    ""Hash"" integer primary key not null,
    ""SuffixClassName"" varchar,
    ""PrefixesClassNames"" varchar,
    ""X"" integer,
    ""Y"" integer )"
, @"CREATE TABLE ""TextToSkillData"" (
    ""TextHash"" integer primary key not null,
    ""SkillHash"" integer)"
};
            foreach (string script in createTableScripts)
            {
                this.Connection.Sql(script).Execute();
            }
        }

        private bool InsertSingleItem<T>(T item) where T : class
        {
            AssertArgument.CheckIsNotNull(item, $"Cannot insert null item [{typeof(T).Name}]");

            int insertedRows = this.Connection
                .Insert<T>(item)
                .Execute() ?? -1;

            return insertedRows == 1;
        }

        private T GetSingleItemByKey<T>(int key) where T : class
        {
            return this.Connection
                .GetByKey<T>(key)
                .ToObjectOrNull()
                .Execute();
        }
    }
}
