using SQLite;
using System;
using System.IO;
using Warcraker.UrbanRivals.DataRepository.DataModels;
using Warcraker.Utils;

namespace Warcraker.UrbanRivals.DataRepository
{
    public class StaticGameDataRepository
    {
        public string Path { get; private set; }

        public StaticGameDataRepository(string path)
        {
            AssertArgument.StringIsFilled(path, nameof(path));
            
            if (!File.Exists(path))
            {
                Initialize(path);
            }

            this.Path = path;
        }

        public BlobCycleData GetBlobCycleData(int blobHash)
        {
            throw new NotImplementedException();
        }
        public SkillData GetSkillData(int skillHash)
        {
            throw new NotImplementedException();
        }
        public ClanData GetClanData(int clanHash)
        {
            throw new NotImplementedException();
        }
        public CardDefinitionData GetCardDefinitionData(int cardDefinitionHash)
        {
            throw new NotImplementedException();
        }
        public int GetSkillHashFromTextHash(int textHash)
        {
            throw new NotImplementedException();
        }

        public void SaveSkillTextHash(int textHash, int skillHash)
        {
            throw new NotImplementedException();
        }

        private static void Initialize(string path)
        {
            using (var connection = new SQLiteConnection(path))
            {
                connection.CreateTable<BlobCycleData>();
                connection.CreateTable<CardDefinitionData>();
                connection.CreateTable<ClanData>();
                connection.CreateTable<SkillData>();
                connection.CreateTable<TextToSkillData>();
            }
        }
    }
}
