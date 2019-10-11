using SQLite;
using System;
using System.IO;
using System.Linq.Expressions;
using Warcraker.UrbanRivals.DataRepository.DataModels;
using Warcraker.Utils;

namespace Warcraker.UrbanRivals.DataRepository
{
    public class GameDataRepository : DataRepository
    {
        public string Path { get; private set; }

        public GameDataRepository(string path)
        {
            AssertArgument.StringIsFilled(path, nameof(path));
            
            if (!File.Exists(path))
            {
                Initialize(path);
            }

            this.Path = path;
        }

        public ClanData GetClanData(int clanHash)
        {
            return GetSingleItem<ClanData>(row => row.Hash == clanHash);
        }
        public CardData GetCardData(int cardHash)
        {
            return GetSingleItem<CardData>(row => row.Hash == cardHash);
        }
        public CycleData GetCycleData(int cycleHash)
        {
            return GetSingleItem<CycleData>(row => row.Hash == cycleHash);
        }
        public SkillData GetSkillData(int skillHash)
        {
            return GetSingleItem<SkillData>(row => row.Hash == skillHash);
        }
        public int GetSkillHashFromTextHash(int textHash)
        {
            TextToSkillData item = GetSingleItem<TextToSkillData>(row => row.TextHash == textHash);

            if (item == null)
                return SCALAR_VALUE_NOT_FOUND;

            return item.SkillHash;
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
        public bool SaveSkillData(SkillData skillData, int skillTextHash)
        {
            AssertArgument.CheckIsNotNull(skillData, $"Cannot insert null item [{typeof(SkillData).Name}]");
            AssertArgument.CheckIntegerRange(skillTextHash > 0, "Must be greater than zero", skillTextHash, nameof(skillTextHash));

            using (var connection = GetConnection())
            {
                TextToSkillData textToSkillData = new TextToSkillData
                {
                    TextHash = skillTextHash,
                    SkillHash = skillData.Hash,
                };

                connection.BeginTransaction();
                connection.Insert(skillData);
                int rowsInserted = connection.Insert(textToSkillData);
                if (rowsInserted > 0)
                {
                    connection.Commit();
                    return true;
                }
                else
                {
                    connection.Rollback();
                    return false;
                }
            }
        }

        private static void Initialize(string path)
        {
            using (var connection = new SQLiteConnection(path))
            {
                connection.CreateTable<CycleData>();
                connection.CreateTable<ClanData>();
                connection.CreateTable<CardData>();
                connection.CreateTable<SkillData>();
                connection.CreateTable<TextToSkillData>();
            }
        }
        
        private SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(Path);
        }
        private T GetSingleItem<T>(Expression<Func<T, bool>> where) where T : new()
        {
            using (var connection = GetConnection())
            {
                T item = connection
                    .Table<T>()
                    .FirstOrDefault(where);

                return item;
            }
        }
        private bool InsertSingleItem<T>(T item)
        {
            AssertArgument.CheckIsNotNull(item, $"Cannot insert null item [{typeof(T).Name}]");
            using (var connection = GetConnection())
            {
                int rowsInserted = connection.Insert(item);

                return rowsInserted > 0;
            }
        }
    }
}
