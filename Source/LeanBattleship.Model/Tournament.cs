using System.Collections.Generic;

namespace LeanBattleship.Model
{
    public class Tournament
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public List<Match> Matches { get; set; }
    }
}
