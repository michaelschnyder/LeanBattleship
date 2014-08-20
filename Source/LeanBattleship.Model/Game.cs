using System.Collections.Generic;

namespace LeanBattleship.Model
{
    public class Game
    {
        public List<Player> Players;

        public Player Winner { get; set; }

        public List<Round> Rounds { get; set; } 
    }
}
