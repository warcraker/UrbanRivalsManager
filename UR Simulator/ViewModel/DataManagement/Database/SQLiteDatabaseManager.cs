using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;

using UrbanRivalsCore.Model;

namespace UrbanRivalsManager.ViewModel.DataManagement
{
    public class SQLiteDatabaseManager : IDatabaseManager
    {
        public SQLiteDatabaseManager(FilepathManager filepathManager)
        {
            FilepathManagerInstance = filepathManager;
            ConnectionStringFormat = "Data Source={0};Version=3;";
            Initialize();
        }

        public void Purge()
        {
            SQLiteConnection.ClearAllPools();
            File.Delete(FilepathManagerInstance.PersistentDatabase);
            File.Delete(FilepathManagerInstance.UserDatabase);
            IsInitialized = false;
            Initialize();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Must be an existing id on the database</param>
        /// <returns></returns>
        public CardBase GetCardBase(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), id, "Must be greater than 0");

            DataTable table;
            SQLiteErrorCode returnCode;

            // Look up for CardLevel's 
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM cardlevel WHERE baseid == ? ORDER BY level;");
            command.Parameters.AddWithValue("@p1", id);
            returnCode = ExecuteSelect(command, FilepathManagerInstance.PersistentDatabase, out table);
            if (returnCode != SQLiteErrorCode.Ok)
                throw new SQLiteException(returnCode, "Failed executing CardLevel SELECT");

            // CardLevel's
            var levels = new List<CardLevel>();
            foreach (DataRow cardLevelRow in table.Rows)
            {
                int level = Convert.ToInt32(cardLevelRow["level"]);
                int power = Convert.ToInt32(cardLevelRow["power"]);
                int damage = Convert.ToInt32(cardLevelRow["damage"]);
                levels.Add(new CardLevel(level, power, damage));
            }

            // Look up for CardBase
            command = new SQLiteCommand("SELECT * FROM cardbase WHERE baseid == ?;");
            command.Parameters.AddWithValue("@p1", id);
            returnCode = ExecuteSelect(command, FilepathManagerInstance.PersistentDatabase, out table);
            if (returnCode != SQLiteErrorCode.Ok)
                throw new SQLiteException(returnCode, "Failed executing CardBase SELECT");
            DataRow row = table.Rows[0];

            // Clan
            ClanId clanid = (ClanId)Convert.ToInt32(row["clanid"]);
            Clan clan = Clan.GetClanById(clanid);

            // Ability
            Skill ability;
            SkillLeader leader = (SkillLeader)Convert.ToInt32(row["leader"]);
            if (leader != SkillLeader.None)
                ability = new Skill(leader);
            else
            {
                SkillSuffix suffix = (SkillSuffix)Convert.ToInt32(row["suffix"]);
                if (suffix == SkillSuffix.None)
                    ability = Skill.NoAbility;
                else
                {
                    SkillPrefix prefix = (SkillPrefix)Convert.ToInt32(row["prefix"]);
                    int x = Convert.ToInt32(row["x"]);
                    int y = Convert.ToInt32(row["y"]);
                    ability = new Skill(prefix, suffix, x, y);
                }
            }

            // Other CardBase details
            string name = row["name"].ToString();
            int minlevel = Convert.ToInt32(row["minlevel"]);
            int maxlevel = Convert.ToInt32(row["maxlevel"]);
            int unlocklevel = Convert.ToInt32(row["unlocklevel"]);
            CardRarity rarity = (CardRarity)Convert.ToInt32(row["rarity"]);
            DateTime releasedate = (Convert.ToDateTime(row["releasedate"]));

            return new CardBase(id, name, clan, minlevel, maxlevel, levels, ability, unlocklevel, rarity, releasedate);
        }
        public IEnumerable<int> GetAllCardBaseIds()
        {
            DataTable table;
            SQLiteErrorCode returnCode;

            SQLiteCommand command = new SQLiteCommand("SELECT baseid FROM cardbase;");
            returnCode = ExecuteSelect(command, FilepathManagerInstance.PersistentDatabase, out table);
            if (returnCode != SQLiteErrorCode.Ok)
                throw new SQLiteException(returnCode, "Failed executing GetAllCardBaseIds SELECT");

            var result = new List<int>();
            foreach (DataRow row in table.Rows)
                yield return Convert.ToInt32(row["baseid"]);
        }
        public bool StoreCardBase(CardBase card)
        {
            if (card == null)
                throw new ArgumentNullException(nameof(card));

            return StoreCardBase(new List<CardBase>() { card });
        }
        public bool StoreCardBase(List<CardBase> list)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            var commands = new List<SQLiteCommand>();
            foreach (var card in list)
            {
                // CardBase
                var command = new SQLiteCommand(
                    "INSERT INTO cardbase (baseid, clanid, name, minlevel, maxlevel, unlocklevel, leader, prefix, suffix, x, y, rarity, releasedate) " +
                    "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?);");
                command.Parameters.AddWithValue("@p1", card.CardBaseId);
                command.Parameters.AddWithValue("@p2", (int)card.Clan.ClanId);
                command.Parameters.AddWithValue("@p3", card.Name);
                command.Parameters.AddWithValue("@p4", card.MinLevel);
                command.Parameters.AddWithValue("@p5", card.MaxLevel);
                command.Parameters.AddWithValue("@p6", card.AbilityUnlockLevel);
                command.Parameters.AddWithValue("@p7", (int)card.Ability.Leader);
                command.Parameters.AddWithValue("@p8", (int)card.Ability.Prefix);
                command.Parameters.AddWithValue("@p9", (int)card.Ability.Suffix);
                command.Parameters.AddWithValue("@p10", card.Ability.X);
                command.Parameters.AddWithValue("@p11", card.Ability.Y);
                command.Parameters.AddWithValue("@p12", (int)card.Rarity);
                command.Parameters.AddWithValue("@p13", card.PublishedDate);
                commands.Add(command);

                // CardLevel's
                for (int level = card.MinLevel; level <= card.MaxLevel; level++)
                {
                    command = new SQLiteCommand(
                        "INSERT INTO cardlevel (baseid, level, power, damage) " +
                        "VALUES (?,?,?,?);");
                    command.Parameters.AddWithValue("@p1", card.CardBaseId);
                    command.Parameters.AddWithValue("@p2", level);
                    command.Parameters.AddWithValue("@p3", card[level].Power);
                    command.Parameters.AddWithValue("@p4", card[level].Damage);
                    commands.Add(command);
                }
            }
            var returnCode = ExecuteTransaction(commands, FilepathManagerInstance.PersistentDatabase);

            if (returnCode != SQLiteErrorCode.Ok)
                return false; // TODO: Log failed Insertion
            return true;
        }

        private bool IsInitialized;
        private FilepathManager FilepathManagerInstance;
        private readonly string ConnectionStringFormat;

        private void Initialize()
        {
            if (IsInitialized)
                return;

            CreatePersistentDatabase();
            CreateUserDatabase();

            IsInitialized = true;
        }
        private bool CreateFile(string path)
        {
            if (File.Exists(path))
                return false;

            SQLiteConnection.CreateFile(path);
            return true;
        }
        private void CreatePersistentDatabase()
        {
            if (!CreateFile(FilepathManagerInstance.PersistentDatabase))
                return;

            using (var connection = new SQLiteConnection(String.Format(ConnectionStringFormat, FilepathManagerInstance.PersistentDatabase)))
            using (var command = new SQLiteCommand(connection))
            {
                connection.Open();

                command.CommandText =
                    "CREATE TABLE cardbase ("
                    + "baseid      INT    PRIMARY KEY NOT NULL,"
                    + "clanid      INT    NOT NULL,"
                    + "name        TEXT   NOT NULL,"
                    + "minlevel    INT    NOT NULL,"
                    + "maxlevel    INT    NOT NULL,"
                    + "unlocklevel INT    NOT NULL,"
                    + "leader      INT,"
                    + "prefix      INT,"
                    + "suffix      INT,"
                    + "x           INT,"
                    + "y           INT,"
                    + "rarity      INT,"
                    + "releasedate DATE"
                    + ")";
                command.ExecuteNonQuery();

                command.CommandText =
                    "CREATE TABLE cardlevel ("
                    + "baseid      INT    NOT NULL,"
                    + "level       INT    NOT NULL,"
                    + "power       INT    NOT NULL,"
                    + "damage      INT    NOT NULL,"
                    + "PRIMARY KEY (baseid, level)"
                    + ")";
                command.ExecuteNonQuery();
            }
        }
        private void CreateUserDatabase()
        {
            if (!CreateFile(FilepathManagerInstance.UserDatabase))
                return;

            using (var connection = new SQLiteConnection(String.Format(ConnectionStringFormat, FilepathManagerInstance.UserDatabase)))
            using (var command = new SQLiteCommand(connection))
            {
                // TODO: DELETE
                //command.CommandText =
                //    "CREATE TABLE cardinstance ("
                //    + "instanceid  INT     PRIMARY KEY NOT NULL,"
                //    + "baseid      INT     NOT NULL,"
                //    + "level       INT     NOT NULL,"
                //    + "experience  INT"
                //    + ")";
                //command.ExecuteNonQuery();
            }
        }

        private SQLiteErrorCode ExecuteTransaction(List<SQLiteCommand> queries, string filepathToDatabase)
        {
            using (var connection = new SQLiteConnection(String.Format(ConnectionStringFormat, filepathToDatabase)))
            {
                connection.Open();
                var transaction = connection.BeginTransaction();
                foreach (var query in queries)
                {
                    query.Connection = connection;
                    query.Transaction = transaction;
                    try
                    {
                        query.ExecuteNonQuery();
                    }
                    catch (SQLiteException ex)
                    {
                        transaction.Rollback();
                        // TODO: Log failed Transaction
                        return ex.ResultCode;
                    }
                }
                transaction.Commit();
                return SQLiteErrorCode.Ok;
            }
        }
        private SQLiteErrorCode ExecuteSelect(SQLiteCommand query, string filepathToDatabase, out DataTable result)
        {
            using (var connection = new SQLiteConnection(String.Format(ConnectionStringFormat, filepathToDatabase)))
            {
                connection.Open();
                query.Connection = connection;
                result = new DataTable();
                new SQLiteDataAdapter(query).Fill(result);
                return SQLiteErrorCode.Ok;
            }
        }
    }
}