using System.Collections.Generic;

namespace LeanBattleship.Model
{
    public class Match
    {
        public Player FirstPlayer { get; set; }

        public Player SecondPlayer { get; set; }

        public Player Winner { get; set; }

        public List<Round> Rounds { get; set; }

        public Fleet FirstPlayerFleet { get; set; }

        public Fleet SecondPlayerFleet { get; set; }
    }
}
