using System;
using System.Collections.Generic;

using UrbanRivalsCore.Model;

namespace UrbanRivalsManager.ViewModel.DataManagement
{
    public class InMemoryManager
    {
        private List<string> CardNames;
        private Dictionary<string, int> CardDefinitionIdsByName;
        private Dictionary<int, CardDefinition> CardDefinitions;
        private Dictionary<int, CardInstance> CardInstances;
        /// <summary>
        /// The key of a fake CardInstance is: (-1) * (CardBaseId * 10 + Level)
        /// </summary>
        private Dictionary<int, CardInstance> FakeCardInstances;

        private InMemoryManager() 
        { 
            CardDefinitions = new Dictionary<int, CardDefinition>();
            CardInstances = new Dictionary<int, CardInstance>();
            FakeCardInstances = new Dictionary<int, CardInstance>();
            CardNames = new List<string>();
            CardDefinitionIdsByName = new Dictionary<string, int>();
        }
        public InMemoryManager(IDatabaseManager databaseManager)
            : this()
        {
            if (databaseManager == null)
                throw new ArgumentNullException(nameof(databaseManager));

            LoadToMemoryFromDatabase(databaseManager);
        }

        public bool LoadToMemoryCardDefinition(CardDefinition card)
        {
            if (CardDefinitions.ContainsKey(card.id))
                return false;

            CardDefinitions[card.id] = card;
            CardDefinitionIdsByName[card.name] = card.id;
            CardNames.Add(card.name);
            LoadToMemoryFakeCardInstances(card);
            return true;
        }
        public void ReloadToMemoryCardInstances(List<CardInstance> instances)
        {
            if (instances == null)
                throw new ArgumentNullException(nameof(instances));

            CardInstances.Clear();
            foreach (CardInstance instance in instances)
                CardInstances[instance.cardInstanceId] = instance;
        }

        public CardDefinition GetCardDefinition(int id)
        {
            return CardDefinitions[id];
        }
        public CardDefinition GetCardDefinition(string name)
        {
            int id = CardDefinitionIdsByName[name];
            return GetCardDefinition(id);
        }
        public CardInstance GetCardInstance(int id)
        {
            return CardInstances[id];
        }
        public CardInstance GetFakeCardInstance(int cardBaseId, int level)
        {
            int id = CalculateFakeInstanceId(cardBaseId, level);
            return FakeCardInstances[id];
        }
        public IEnumerable<CardDefinition> SearchCardsByName(string partialName)
        {
            foreach (string name in LookForCardNames(partialName))
                yield return GetCardDefinition(name);
        }
        public IEnumerable<string> LookForCardNames(string partialName)
        {
            if (String.IsNullOrEmpty(partialName))
                return CardNames.FindAll(name => true);
            else
                return CardNames.FindAll(name => name.IndexOf(partialName, StringComparison.OrdinalIgnoreCase) >= 0);
        }
        public IEnumerable<int> GetAllCardBaseIds()
        {
            foreach (int id in CardDefinitions.Keys)
                yield return id;
        }
        public IEnumerable<CardDefinition> GetAllCardBases()
        {
            foreach (CardDefinition card in CardDefinitions.Values)
                yield return card;
        }

        private void LoadToMemoryFromDatabase(IDatabaseManager databaseManager)
        {
            var ids = databaseManager.getAllCardDefinitionIds();
            foreach (int id in ids)
                LoadToMemoryCardDefinition(databaseManager.getCardDefinitionById(id));
        }
        private void LoadToMemoryFakeCardInstances(CardDefinition card)
        {
            for (int level = card.minLevel; level <= card.maxLevel; level++)
            {
                int id = CalculateFakeInstanceId(card.id, level);
                if (level == card.maxLevel)
                {
                    FakeCardInstances[id] = CardInstance.createCardInstanceAtMaxLevel(card, id);
                }
                else
                {
                    FakeCardInstances[id] = CardInstance.createCardInstance(card, id, level, 0);
                }
            }
        }

        private static int CalculateFakeInstanceId(int cardBaseId, int level)
        {
            return (-1) * (cardBaseId * 10 + level);
        }
    }
}