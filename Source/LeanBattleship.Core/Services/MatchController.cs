using LeanBattleship.Common;
using LeanBattleship.Core.Game;
using LeanBattleship.Data;
using LeanBattleship.Model;

namespace LeanBattleship.Core.Services
{
    class MatchController : IMatchController
    {
        private readonly int matchId;
        private readonly DataContext context;

        public MatchController(int matchId, DataContext context)
        {
            this.matchId = matchId;
            this.context = context;
        }

        public bool Fire(Player player, string position)
        {
            var serializer = new GameFleetSerializer();

            // Convert to x/y
            var colString = position.ToLower()[0];

            var colValue = colString - 65;
            var rowValue = int.Parse(position[0].ToString());

            var match = this.context.Matches.Find(this.matchId);
            var wasHit = false;

            if (match.CurrentPlayer == player)
            {
                if (player == match.FirstPlayer)
                {
                    var oppositePlayerFleet = serializer.Deserialize(match.SecondPlayerFleet.RawFleetValue);
                    wasHit = oppositePlayerFleet.Fire(rowValue, colValue);
                    match.SecondPlayerFleet.RawFleetValue = serializer.Serialize(oppositePlayerFleet);

                    if (!wasHit)
                    {
                        match.CurrentPlayer = match.SecondPlayer;
                    }
                }
                else
                {
                    var oppositePlayerFleet = serializer.Deserialize(match.FirstPlayerFleet.RawFleetValue);
                    wasHit = oppositePlayerFleet.Fire(rowValue, colValue);
                    match.FirstPlayerFleet.RawFleetValue = serializer.Serialize(oppositePlayerFleet);

                    if (!wasHit)
                    {
                        match.CurrentPlayer = null;
                    }
                }
            }

            return wasHit;
        }
    }
}