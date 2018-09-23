using System;
using System.Windows;
using System.Collections.Generic;
using System.ComponentModel;

using UrbanRivalsCore.Model;

namespace UrbanRivalsCore.Model
{
    /// <summary>
    /// Keeps track of the current status of a player.
    /// </summary>
    public class PlayerStatus : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int m_Life;
        /// <summary>
        /// Gets or sets the current life. 
        /// <remarks>Can't be negative.</remarks>
        /// </summary>
        public int Life
        {
            get { return m_Life; }
            set
            {
                if (value < 0)
                    return;
                m_Life = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Life)));
            }
        }
        private int m_Pillz;
        /// <summary>
        /// Gets or sets the current pillz. 
        /// <remarks>Can't be negative.</remarks>
        /// </summary>
        public int Pillz
        {
            get { return m_Pillz; }
            set
            {
                if (value < 0)
                    return;
                m_Pillz = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Pillz)));
            }
        }

        private Heal m_Heal;
        /// <summary>
        /// Gets or sets the heal status.
        /// </summary>
        public Heal Heal
        {
            get { return m_Heal; }
            set
            {
                if (value == null)
                    return;
                m_Heal = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Heal)));
            }
        }
        private Poison m_Poison;
        /// <summary>
        /// Gets or sets the poison status.
        /// </summary>
        public Poison Poison
        {
            get { return m_Poison; }
            set
            {
                if (value == null)
                    return;
                m_Poison = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Poison)));
            }
        }

        private bool m_Courage;
        private bool m_Confidence;
        private bool m_Revenge;

        /// <summary>
        /// Gets or sets if the player has courage.
        /// </summary>
        public bool Courage
        {
            get { return m_Courage; }
            set
            {
                if (m_Courage == value)
                    return;

                m_Courage = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Courage)));
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Reprisal)));
                }
            }
        }
        /// <summary>
        /// Gets or sets if the player has reprisal.
        /// </summary>
        public bool Reprisal
        {
            get { return !m_Courage; }
            set
            {
                if (m_Courage == !value)
                    return;

                m_Courage = !value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Reprisal)));
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Courage)));
                }
            }
        }
        /// <summary>
        /// Gets or sets if the player has confidence.
        /// </summary>
        public bool Confidence
        {
            get { return m_Confidence; }
            set
            {
                if (m_Confidence == value)
                    return;

                m_Confidence = value;
                if (value)
                    Revenge = false;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Confidence)));
            }
        }
        /// <summary>
        /// Gets or sets if the player has revenge.
        /// </summary>
        public bool Revenge
        {
            get { return m_Revenge; }
            set
            {
                if (m_Revenge == value)
                    return;

                m_Revenge = value;
                if (value)
                    Confidence = false;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Revenge)));
            }
        }

        private Hand m_Hand;
        /// <summary>
        /// Gets the hand.
        /// </summary>
        public Hand Hand
        {
            get { return m_Hand; }
            private set
            {
                if (value == null)
                    return;

                m_Hand = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Hand)));
            }
        }

        // Constructors

        private PlayerStatus() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerStatus"/> class. 
        /// </summary>
        /// <param name="initialLife">Starting lives.</param>
        /// <param name="initialPillz">Starting pillz.</param>
        /// <param name="hand">Hand drawn.</param>
        /// <param name="isFirstPlayer">Is this player the first one to play on first round?</param>
        public PlayerStatus(int initialLife, int initialPillz, Hand hand, bool isFirstPlayer)
        {
            if (initialLife <= 0)
                throw new ArgumentOutOfRangeException(nameof(initialLife), initialLife, "Must be greater than 0");
            if (initialPillz < 0)
                throw new ArgumentOutOfRangeException(nameof(initialPillz), initialPillz, "Must be greater or equal to 0");
            if (hand == null)
                throw new ArgumentNullException(nameof(hand));

            Life = initialLife;
            Pillz = initialPillz;
            Hand = hand;
            Courage = isFirstPlayer;
            Heal = new Heal();
            Poison = new Poison();
        }

        // Functions

        /// <summary>
        /// Returns a copy of this instance.
        /// </summary>
        /// <returns></returns>
        public PlayerStatus Copy()
        {
            var result = new PlayerStatus();

            result.Life = this.Life;
            result.Pillz = this.Life;

            result.Hand = this.Hand.Copy();

            result.Courage = this.Courage;
            result.Reprisal = this.Reprisal;
            result.Confidence = this.Confidence;
            result.Revenge = this.Revenge;

            result.Heal = this.Heal.Copy();
            result.Poison = this.Poison.Copy();

            return result;
        }
    }
}