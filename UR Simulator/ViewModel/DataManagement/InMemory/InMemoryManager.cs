using System;
using System.Collections.Generic;

using UrbanRivalsCore.Model;

namespace UrbanRivalsManager.ViewModel.DataManagement
{
    public class InMemoryManager
    {
        private List<string> CardNames;
        private Dictionary<string, int> CardBaseIdsByName;
        private Dictionary<int, CardBase> CardBases;
        private Dictionary<int, CardInstance> CardInstances;
        /// <summary>
        /// The key of a fake CardInstance is: (-1) * (CardBaseId * 10 + Level)
        /// </summary>
        private Dictionary<int, CardInstance> FakeCardInstances;

        private InMemoryManager() 
        { 
            CardBases = new Dictionary<int, CardBase>();
            CardInstances = new Dictionary<int, CardInstance>();
            FakeCardInstances = new Dictionary<int, CardInstance>();
            CardNames = new List<string>();
            CardBaseIdsByName = new Dictionary<string, int>();
        }
        public InMemoryManager(IDatabaseManager databaseManager)
            : this()
        {
            if (databaseManager == null)
                throw new ArgumentNullException(nameof(databaseManager));

            LoadToMemoryFromDatabase(databaseManager);
        }

        public bool LoadToMemoryCardBase(CardBase card)
        {
            if (CardBases.ContainsKey(card.cardBaseId))
                return false;

            CardBases[card.cardBaseId] = card;
            CardBaseIdsByName[card.name] = card.cardBaseId;
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

        public CardBase GetCardBase(int id)
        {
            return CardBases[id];
        }
        public CardBase GetCardBase(string name)
        {
            int id = CardBaseIdsByName[name];
            return GetCardBase(id);
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
        public IEnumerable<CardBase> SearchCardsByName(string partialName)
        {
            foreach (string name in LookForCardNames(partialName))
                yield return GetCardBase(name);
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
            foreach (int id in CardBases.Keys)
                yield return id;
        }
        public IEnumerable<CardBase> GetAllCardBases()
        {
            foreach (CardBase card in CardBases.Values)
                yield return card;
        }

        private void LoadToMemoryFromDatabase(IDatabaseManager databaseManager)
        {
            var ids = databaseManager.getAllCardBaseIds();
            foreach (int id in ids)
                LoadToMemoryCardBase(databaseManager.getCardBase(id));
        }
        private void LoadToMemoryFakeCardInstances(CardBase card)
        {
            for (int level = card.minLevel; level <= card.maxLevel; level++)
            {
                int id = CalculateFakeInstanceId(card.cardBaseId, level);
                FakeCardInstances[id] = CardInstance.createCardInstance(card, id, level, 0);
            }
        }

        private static int CalculateFakeInstanceId(int cardBaseId, int level)
        {
            return (-1) * (cardBaseId * 10 + level);
        }
    }
}