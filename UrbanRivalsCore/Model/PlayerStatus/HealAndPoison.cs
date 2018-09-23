using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanRivalsCore.Model
{
    /// <summary>
    /// Represents the heal status of a player.
    /// </summary>
    public class Heal : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int m_Max;
        /// <summary>
        /// Gets or sets the maximum limit.
        /// </summary>
        public int Max
        {
            get { return m_Max; }
            set
            {
                if (value < 0)
                    return;
                m_Max = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Max)));
            }
        }

        private int m_Value;
        /// <summary>
        /// Gets or sets the amount of lives the player will recover per round.
        /// </summary>
        public int Value
        {
            get { return m_Value; }
            set
            {
                if (value < 0)
                    return;
                m_Value = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Value)));
            }
        }

        // Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Heal"/> class with its values set to 0.
        /// </summary>
        public Heal()
        {
            Max = 0;
            Value = 0;
        }

        public Heal(Skill skill)
        {
            if (skill == null)
                throw new ArgumentNullException(nameof(skill));
            if (skill.Suffix != SkillSuffix.HealXMaxY || skill.Suffix != SkillSuffix.RegenXMaxY)
                throw new ArgumentException("Must be a heal or regen type of skill", nameof(skill));

            Value = skill.X;
            Max = skill.Y;
        }

        // Functions

        /// <summary>
        /// Returns a copy of this instance.
        /// </summary>
        /// <returns></returns>
        public Heal Copy()
        {
            return new Heal()
            {
                Max = this.Max,
                Value = this.Value
            };
        }
    }

    /// <summary>
    /// Represents the poison status of a player.
    /// </summary>
    public class Poison : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int m_Min;
        /// <summary>
        /// Gets or sets the minimum limit.
        /// </summary>
        public int Min
        {
            get { return m_Min; }
            set
            {
                if (value < 0)
                    return;
                m_Min = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Min)));
            }
        }

        private int m_Value;
        /// <summary>
        /// Gets or sets the amount of lives the player will lose per round.
        /// </summary>
        public int Value
        {
            get { return m_Value; }
            set
            {
                if (value < 0)
                    return;
                m_Value = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Value)));
            }
        }

        // Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Poison"/> class with its values set to 0. 
        /// </summary>
        public Poison()
        {
            Min = 0;
            Value = 0;
        }

        public Poison(Skill skill)
        {
            if (skill == null)
                throw new ArgumentNullException(nameof(skill));
            if (skill.Suffix != SkillSuffix.PoisonXMinY || skill.Suffix != SkillSuffix.ToxinXMinY)
                throw new ArgumentException("Must be a poison or toxin type of skill", nameof(skill));

            Value = skill.X;
            Min = skill.Y;
        }
        // Functions

        /// <summary>
        /// Returns a copy of this instance.
        /// </summary>
        /// <returns></returns>
        public Poison Copy()
        {
            return new Poison()
            {
                Min = this.Min,
                Value = this.Value
            };
        }
    }
}
