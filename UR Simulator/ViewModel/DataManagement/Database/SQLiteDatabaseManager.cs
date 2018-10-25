using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using UrbanRivalsCore.Model;
using UrbanRivalsUtils;

namespace UrbanRivalsManager.ViewModel.DataManagement
{
    public class SQLiteDatabaseManager : IDatabaseManager
    {
        private const string PRV_CONNECITON_STRING_FORMAT = "Data Source={0};Version=3;";
        private const string PRV_SELECT_CARDLEVELS_BY_ID = "SELECT * FROM cardlevel WHERE baseid == ? ORDER BY level;";
        private const string PRV_SELECT_CARDBASE_BY_ID = "SELECT * FROM cardbase WHERE baseid == ?;";

        private readonly string persistentDatabasePath;

        public SQLiteDatabaseManager(string persistentDatabasePath)
        {
            this.persistentDatabasePath = persistentDatabasePath;
            prv_createPersistentDatabase(this.persistentDatabasePath);
        }

        public void purge()
        {
            SQLiteConnection.ClearAllPools();
            File.Delete(this.persistentDatabasePath);
            prv_createPersistentDatabase(this.persistentDatabasePath);
        }

        public CardBase getCardBase(int id)
        {
            CardBase card;
            List<CardLevel> levels;
            Clan clan;
            Skill ability;
            string name;
            int unlocklevel;
            CardRarity rarity;

            AssertArgument.checkIntegerRange(id <= 0, "Must be greater than 0", id, nameof(id));

            using (SQLiteCommand command = new SQLiteCommand(PRV_SELECT_CARDLEVELS_BY_ID))
            {
                DataTable table;

                prv_addParameter(command, 1, id);
                table = prv_executeSelect(command, this.persistentDatabasePath);
                levels = new List<CardLevel>();
                foreach (DataRow cardLevelRow in table.Rows)
                {
                    int level, power, damage;

                    level = prv_getInteger(cardLevelRow, "level");
                    power = prv_getInteger(cardLevelRow, "power");
                    damage = prv_getInteger(cardLevelRow, "damage");
                    levels.Add(new CardLevel(level, power, damage));
                }
            }

            using (SQLiteCommand command = new SQLiteCommand(PRV_SELECT_CARDBASE_BY_ID))
            {
                DataTable table;
                DataRow row;
                ClanId clanid;
                SkillLeader leader;
                SkillSuffix suffix;

                command.Parameters.AddWithValue("@p1", id);
                table = prv_executeSelect(command, this.persistentDatabasePath);
                row = table.Rows[0];

                clanid = prv_getEnum<ClanId>(row, "clanid");
                clan = Clan.getClanById(clanid);

                leader = prv_getEnum<SkillLeader>(row, "leader");
                suffix = prv_getEnum<SkillSuffix>(row, "suffix");
                if (leader != SkillLeader.None)
                {
                    ability = new Skill(leader);
                }
                else if (suffix == SkillSuffix.None)
                {
                    ability = Skill.NoAbility;
                }
                else
                {
                    SkillPrefix prefix;
                    int x, y;

                    prefix = prv_getEnum<SkillPrefix>(row, "prefix");
                    x = prv_getInteger(row, "x");
                    y = prv_getInteger(row, "y");
                    ability = new Skill(prefix, suffix, x, y);
                }

                name = prv_getString(row, "name");
                unlocklevel = prv_getInteger(row, "unlocklevel");
                rarity = prv_getEnum<CardRarity>(row, "rarity");
            }

            card = CardBase.createCardWithAbility(id, name, clan, levels, rarity, ability, unlocklevel);

            return card;
        }
        public IEnumerable<int> getAllCardBaseIds()
        {
            DataTable table;

            using (SQLiteCommand command = new SQLiteCommand("SELECT baseid FROM cardbase;"))
            {
                table = prv_executeSelect(command, this.persistentDatabasePath);
            }

            foreach (DataRow row in table.Rows)
            {
                int returnedId;

                returnedId = prv_getInteger(row, "baseid");
                yield return returnedId;
            }
        }
        public void storeCardBase(CardBase card)
        {
            List<SQLiteCommand> commands;
            SQLiteCommand addCardBaseCommand;
            int cardId;
            Clan clan;
            int clanId;
            string name;
            int unlockLevel;
            Skill ability;
            int leader;
            int prefix;
            int suffix;
            int x, y;
            int rarity;

            AssertArgument.isNotNull(card, nameof(card));

            commands = new List<SQLiteCommand>();

            cardId = card.cardBaseId;
            clan = card.clan;
            clanId = (int)clan.id;
            name = card.name;
            unlockLevel = card.abilityUnlockLevel;
            ability = card.ability;
            leader = (int)ability.Leader;
            prefix = (int)ability.Prefix;
            suffix = (int)ability.Suffix;
            x = ability.X;
            y = ability.Y;
            rarity = (int)card.rarity;

            addCardBaseCommand = new SQLiteCommand(
                "INSERT INTO cardbase (baseid, clanid, name, unlocklevel, leader, prefix, suffix, x, y, rarity) " +
                "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?);");
            prv_addParameter(addCardBaseCommand, 1, cardId);
            prv_addParameter(addCardBaseCommand, 2, clanId);
            prv_addParameter(addCardBaseCommand, 3, name);
            prv_addParameter(addCardBaseCommand, 4, unlockLevel);
            prv_addParameter(addCardBaseCommand, 5, leader);
            prv_addParameter(addCardBaseCommand, 6, prefix);
            prv_addParameter(addCardBaseCommand, 7, suffix);
            prv_addParameter(addCardBaseCommand, 8, x);
            prv_addParameter(addCardBaseCommand, 9, y);
            prv_addParameter(addCardBaseCommand, 10, rarity);
            commands.Add(addCardBaseCommand);

            for (int currentLevel = card.minLevel, max = card.maxLevel; currentLevel <= max; currentLevel++)
            {
                SQLiteCommand addCardLevelCommand;
                CardLevel cardLevel;
                int power;
                int damage;

                cardLevel = card.getCardLevel(currentLevel);
                power = cardLevel.power;
                damage = cardLevel.damage;

                addCardLevelCommand = new SQLiteCommand(
                    "INSERT INTO cardlevel (baseid, level, power, damage) " +
                    "VALUES (?,?,?,?);");
                prv_addParameter(addCardLevelCommand, 1, cardId);
                prv_addParameter(addCardLevelCommand, 2, currentLevel);
                prv_addParameter(addCardLevelCommand, 3, power);
                prv_addParameter(addCardLevelCommand, 4, damage);
                commands.Add(addCardLevelCommand);
            }
            prv_executeCommandsAsTransaction(commands, this.persistentDatabasePath);
        }

        private static bool prv_tryCreateFile(string path)
        {
            bool fileCreated;

            if (File.Exists(path))
            {
                fileCreated = false;
            }
            else
            {
                SQLiteConnection.CreateFile(path);
                fileCreated = true;
            }

            return fileCreated;
        }
        private static void prv_createPersistentDatabase(string databasePath)
        {
            bool databaseNeedsToBeRecreated;

            databaseNeedsToBeRecreated = prv_tryCreateFile(databasePath);
            if (databaseNeedsToBeRecreated == true)
            {
                string connectionString;

                connectionString = String.Format(PRV_CONNECITON_STRING_FORMAT, databasePath);
                using (var connection = new SQLiteConnection(connectionString))
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
        }

        private static string prv_getString(DataRow row, string columnName)
        {
            string value;
            object valueAsObject;

            valueAsObject = row[columnName];
            value = valueAsObject.ToString();

            return value;
        }
        private static int prv_getInteger(DataRow row, string columnName)
        {
            int value;
            object valueAsObject;

            valueAsObject = row[columnName];
            value = Convert.ToInt32(valueAsObject);

            return value;
        }
        private static T prv_getEnum<T>(DataRow row, string columnName) where T : Enum
        {
            T value;
            int valueAsInteger;
            object valueAsObject;

            valueAsInteger = prv_getInteger(row, columnName);
            valueAsObject = (object)valueAsInteger;
            value = (T)valueAsObject;

            return value;
        }
        private static void prv_addParameter<T>(SQLiteCommand command, int parameterIndex, T value)
        {
            command.Parameters.AddWithValue($"@p{parameterIndex}", value);
        }
        private static void prv_executeCommandsAsTransaction(List<SQLiteCommand> commands, string filepathToDatabase)
        {
            using (SQLiteConnection connection = new SQLiteConnection(String.Format(PRV_CONNECITON_STRING_FORMAT, filepathToDatabase)))
            {
                connection.Open();

                using (SQLiteTransaction transaction = connection.BeginTransaction())
                {
                    foreach (SQLiteCommand command in commands)
                    {
                        command.Connection = connection;
                        command.Transaction = transaction;
                        try
                        {
                            command.ExecuteNonQuery();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                    transaction.Commit();
                }
            }
        }
        private static DataTable prv_executeSelect(SQLiteCommand query, string filepathToDatabase)
        {
            DataTable table;

            using (SQLiteConnection connection = new SQLiteConnection(String.Format(PRV_CONNECITON_STRING_FORMAT, filepathToDatabase)))
            {
                SQLiteDataAdapter adapter;

                connection.Open();
                query.Connection = connection;
                table = new DataTable();
                adapter = new SQLiteDataAdapter(query);
                adapter.Fill(table);
            }

            return table;
        }
    }
}