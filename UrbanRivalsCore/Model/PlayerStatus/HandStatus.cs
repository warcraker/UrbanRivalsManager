using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace UrbanRivalsCore.Model
{
    /// <summary>
    /// Keeps track of played cards, pillz, rounds and fury.
    /// </summary>
    public class HandStatus : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private List<int?> _UsedCardNumberPerRound;
        /// <summary>
        /// Gets the played card per round. <c>null</c> means that round has not been played.
        /// <example>[2, 1, null, null] means that the third card was played on round 1, then the second card on round 2.</example>
        /// </summary>
        public ReadOnlyCollection<int?> UsedCardNumberPerRound { get { return _UsedCardNumberPerRound.AsReadOnly(); } }

        private List<bool> _HasCardBeingPlayed;
        /// <summary>
        /// Gets the played state per cards.
        /// <example>[false, true, true, false] means that the second and third cards were played.</example>
        /// </summary>
        public ReadOnlyCollection<bool> HasCardBeingPlayed { get { return _HasCardBeingPlayed.AsReadOnly(); } }

        private List<bool?> _HasWonPerRound;
        /// <summary>
        /// Gets <c>true</c> if the player won that round. <c>false</c> if lost. <c>null</c> means that round has not been played.
        /// <example>[true, false, null, null] means that the round 1 was won, the round 2 was lost, and rounds 3 and 4 haven't been played.</example>
        /// </summary>
        public ReadOnlyCollection<bool?> HasWonPerRound { get { return _HasWonPerRound.AsReadOnly(); } }

        private List<int?> _UsedPillzPerRound;
        /// <summary>
        /// Gets the amount of pillz used per round. Fury not included. <c>null</c> means that round has not been played.
        /// <example>[3, 0, null, null] means that 3 pillz were used on round 1, and 0 pillz were used on round 2. Rounds 3 and 4 hasn't been played yet.</example>
        /// </summary>
        public ReadOnlyCollection<int?> UsedPillzPerRound { get { return _UsedPillzPerRound.AsReadOnly(); } }

        private List<bool?> _UsedFuryPerRound;
        /// <summary>
        /// Gets the fury used state per round. <c>null</c> means that round has not been played.
        /// <example>[false, true, null, null] means fury used on round 2. Rounds 3 and 4 hasn't been played yet.</example>
        /// </summary>
        public ReadOnlyCollection<bool?> UsedFuryPerRound { get { return _UsedFuryPerRound.AsReadOnly(); } }

        private List<bool?> _HasWonPerCard;
        /// <summary>
        /// Gets <c>true</c> if that card won its round. <c>false</c> if lost. <c>null</c> means that card has not been played.
        /// <example>[null, false, true, null] means that card 2 lost its round, card 3 won its round, and cards 1 and 4 haven't been played.</example>
        /// </summary>
        public ReadOnlyCollection<bool?> HasWonPerCard { get { return _HasWonPerCard.AsReadOnly(); } }

        private List<int?> _UsedPillzPerCard;
        /// <summary>
        /// Gets the used pillz per card. Fury not included. <c>null</c> means that card has not been played.
        /// <example>[null, 0, 3, null] means that 0 pillz were used on the second card, and 3 pillz were used on the third card.</example>
        /// </summary>
        public ReadOnlyCollection<int?> UsedPillzPerCard { get { return _UsedPillzPerCard.AsReadOnly(); } }

        private List<bool?> _UsedFuryPerCard;
        /// <summary>
        /// Gets the fury used state per card. <c>null</c> means that card has not been played.
        /// [null, true, false, null] means fury was used on the second card. First and fourth cards haven't been played.
        /// </summary>
        public ReadOnlyCollection<bool?> UsedFuryPerCard { get { return _UsedFuryPerCard.AsReadOnly(); } }

        // Constructors

        internal HandStatus()
        {
            _UsedCardNumberPerRound = new List<int?>() { null, null, null, null };
            _HasCardBeingPlayed = new List<bool>() { false, false, false, false };
            _HasWonPerRound = new List<bool?>() { null, null, null, null };
            _UsedPillzPerRound = new List<int?>() { null, null, null, null };
            _UsedFuryPerRound = new List<bool?>() { null, null, null, null };
            _HasWonPerCard = new List<bool?>() { null, null, null, null };
            _UsedPillzPerCard = new List<int?>() { null, null, null, null };
            _UsedFuryPerCard = new List<bool?>() { null, null, null, null };
        }

        // Functions

        internal void SetRoundData(int round, int usedCard, bool usedFury, int usedPillz, bool hasWon)
        {
            if (round < 0 || round > 3)
                throw new ArgumentOutOfRangeException(nameof(round), round, "Must be between 0 and 3 inclusive");
            if (usedCard < 0 || usedCard > 3)
                throw new ArgumentOutOfRangeException(nameof(usedCard), usedCard, "Must be between 0 and 3 inclusive");
            if (usedPillz < 0)
                throw new ArgumentOutOfRangeException(nameof(usedPillz), usedPillz, "Must be greater than or equal to 0");

            _UsedCardNumberPerRound[round] = usedCard;
            _HasCardBeingPlayed[usedCard] = true;
            _HasWonPerRound[round] = hasWon;
            _UsedPillzPerRound[round] = usedPillz;
            _UsedFuryPerRound[round] = usedFury;
            _HasWonPerCard[usedCard] = hasWon;
            _UsedPillzPerCard[usedCard] = usedPillz;
            _UsedFuryPerCard[usedCard] = usedFury;

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(UsedCardNumberPerRound)));
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(HasCardBeingPlayed)));
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(HasWonPerRound)));
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(UsedPillzPerRound)));
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(UsedFuryPerRound)));
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(HasWonPerCard)));
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(UsedPillzPerCard)));
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(UsedFuryPerCard)));
            }
        }

        internal HandStatus Copy()
        {
            HandStatus copy = new HandStatus();
            for (int round = 0; round <= 3; round++)
            {
                if (_UsedCardNumberPerRound[round] == null)
                    break;

                int usedCard = (int)_UsedCardNumberPerRound[round];
                int usedPillz = (int)_UsedPillzPerRound[round];
                bool usedFury = (bool)_UsedFuryPerRound[round];
                bool hasWon = (bool)_HasWonPerRound[round];
                copy.SetRoundData(round, usedCard, usedFury, usedPillz, hasWon);
            }
            return copy;
        }
    }
}