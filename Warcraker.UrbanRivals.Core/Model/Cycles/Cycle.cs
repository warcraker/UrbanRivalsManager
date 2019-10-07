using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Warcraker.UrbanRivals.Core.Model.Cards;

namespace Warcraker.UrbanRivals.Core.Model.Cycles
{
    public class Cycle
    {
        public enum ECycleType
        {
            Day = 0,
            Night,
        }

        public ECycleType CycleType { get; set; }
        public IReadOnlyDictionary<int, Clan> Clans { get; private set; }
        public IReadOnlyDictionary<int, CardDefinition> Cards { get; private set; }

        public Cycle(ECycleType cycleType, IEnumerable<CardDefinition> cards)
        {
            var clans = cards
                .Select(card => card.Clan)
                .GroupBy(clan => clan.GameId)
                .Select(group => group.First());

            this.CycleType = cycleType;
            this.Clans = new ReadOnlyDictionary<int, Clan>(clans.ToDictionary(clan => clan.GameId));
            this.Cards = new ReadOnlyDictionary<int, CardDefinition>(cards.ToDictionary(card => card.GameId));
        }
    }
}
