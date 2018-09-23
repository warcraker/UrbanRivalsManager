using System;
using System.Collections.Generic;
using System.Linq;

namespace UrbanRivalsCore.Model
{
    public class Deck
    {
        /// <summary>
        /// Gets the identifier used by UR to identify the deck.
        /// </summary>
        public int DeckId { get; private set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public String Name { get; private set; }

        private List<CardInstance> _cards;
        /// <summary>
        /// Gets the cards on the deck.
        /// </summary>
        public List<CardInstance> Cards { get { return _cards.ToList(); } }

        // Derived info

        /// <summary>
        /// Gets the number of cards on the deck.
        /// </summary>
        public int Size { get { return _cards.Count; } }

        // Constructors

        private Deck() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Deck"/> class.
        /// </summary>
        /// <param name="deckId">Unique identifier.</param>
        /// <param name="name">Name.</param>
        /// <param name="cards">Cards on the deck.</param>
        public Deck(int deckId, string name, IEnumerable<CardInstance> cards)
        {
            if (deckId == 0)
                throw new ArgumentException("Can't be 0", nameof(deckId));
            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));
            if (cards == null)
                throw new ArgumentNullException(nameof(cards));
            if (cards.Count() < 8)
                throw new ArgumentException("Must have a minimum of 8 cards", nameof(cards));

            DeckId = deckId;
            Name = name;
            _cards = cards.ToList();
        }

        // Functions

        /// <summary>
        /// Returns 4 random cards from the deck.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CardDrawed> DrawCards()
        {
            List<int> posiblePositions = new List<int>(Size);
            for (int i = 0; i < Size; i++)
                posiblePositions.Add(i);

            for (int i = 0; i < 4; i++)
            {
                int pos = GlobalRandom.Next(posiblePositions.Count - 1);
                yield return new CardDrawed(_cards[posiblePositions.ElementAt<int>(pos)]);
                posiblePositions.RemoveAt(pos);
            }
        }

        /// <summary>
        /// Adds the card to the deck.
        /// </summary>
        /// <param name="card">Card to be added.</param>
        /// <returns><c>false</c> if the card already exists on the deck, and the card will not be added. <c>true</c> otherwise.</returns>
        public bool AddCard(CardInstance card)
        {
            if (card == null)
                throw new ArgumentNullException(nameof(card));

            if (_cards.Contains(card))
                return false;

            _cards.Add(card);
            return true;
        }

        /// <summary>
        /// Removes the card from the <see cref="Deck"/>.
        /// </summary>
        /// <param name="instanceId">Card Instance Id.</param>
        /// <returns><c>false</c> if the card doesn't exists, or the deck has 8 cards, and the card will not be removed. <c>true</c> otherwise</returns>
        public bool RemoveCard(int instanceId)
        {
            CardInstance card = Cards.FirstOrDefault(a => a.CardInstanceId == instanceId);

            return this.RemoveCard(card);
        }

        /// <summary>
        /// Removes the card from the <see cref="Deck"/>.
        /// </summary>
        /// <param name="card">Card.</param>
        /// <returns><c>false</c> if the card doesn't exists, or the deck has 8 cards, and the card will not be removed. <c>true</c> otherwise</returns>
        private bool RemoveCard(CardInstance card)
        {
            if (card == null || Size == 8)
                return false;

            return _cards.Remove(card);
        }
    }
}
