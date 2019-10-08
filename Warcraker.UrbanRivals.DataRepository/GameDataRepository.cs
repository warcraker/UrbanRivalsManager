using SQLite;
using System;
using System.IO;
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
            throw new NotImplementedException();
        }
        public CardData GetCardData(int cardHash)
        {
            throw new NotImplementedException();
        }
        public CycleData GetCycleData(int cycleHash)
        {
            throw new NotImplementedException();
        }
        public SkillData GetSkillData(int skillHash)
        {
            throw new NotImplementedException();
        }
        public int GetSkillHashFromTextHash(int textHash)
        {
            throw new NotImplementedException();
        }

        public void SaveClanData(ClanData clanData)
        {
            throw new NotImplementedException();
        }
        public void SaveCardData(CardData cardData)
        {
            throw new NotImplementedException();
        }
        public void SaveCycleBlobData(CycleData blobData)
        {
            throw new NotImplementedException();
        }
        public void SaveSkillData(SkillData skillData)
        {
            throw new NotImplementedException();
        }
        public void SaveSkillTextHash(int textHash, int skillHash)
        {
            TextToSkillData data = new TextToSkillData
            {
                TextHash = textHash,
                SkillHash = skillHash,
            };

            throw new NotImplementedException();
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

    }
}
