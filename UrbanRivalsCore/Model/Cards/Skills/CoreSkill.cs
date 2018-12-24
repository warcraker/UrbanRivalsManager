using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrbanRivalsCore.Model.Cards.Skills
{
    public class CoreSkill
    {
        public Func<Hand, int, Hand, int, int, int> Func_CalculateMultiplier { get; set; }

        public int X { get; set; }
        public int Y { get; set; }

        public CoreSkill()
        {
            this.X = 0;
            this.Y = 0;
            this.Func_CalculateMultiplier = new Func<Hand, int, Hand, int, int, int>((ownHand, ownSelectedCard, enemyHand, enemySelectedCard, round) => 1);
        }
    }
}
