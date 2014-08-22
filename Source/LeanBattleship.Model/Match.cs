using System;
using System.Collections.Generic;

namespace LeanBattleship.Model
{
    public class Match
    {
        public long Id { get; set; }

        public MatchState State { get; set; }

        public Player FirstPlayer { get; set; }

        public Player SecondPlayer { get; set; }

        public Player CurrentPlayer { get; set; }

        public Player Winner { get; set; }

        public List<Round> Rounds { get; set; }

        public string FirstPlayerFleetRaw { get; set; }

        public string SecondPlayerFleetRaw { get; set; }

        public DateTime SetupTimeUtc { get; set; }
        
        public DateTime? StartTimeUtc { get; set; }

        public DateTime? LastFireUtc { get; set; }

    }

    public enum MatchState
    {
        Setup = 1001,
        Started = 1002,
        Finished = 1003,
        Canceled = 1009
    }
}
