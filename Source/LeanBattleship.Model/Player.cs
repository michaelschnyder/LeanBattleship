using System.Collections.Generic;

namespace LeanBattleship.Model
{
    public class Player
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string CallbackUrl { get; set; }

        public List<Tournament> Tournaments { get; set; }
    }
}
