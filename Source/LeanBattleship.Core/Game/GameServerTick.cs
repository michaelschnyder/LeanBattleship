using System.Linq;
using LeanBattleship.Common;
using LeanBattleship.Data;
using LeanBattleship.Model;
using Microsoft.Practices.ServiceLocation;

namespace LeanBattleship.Core.Game
{
    internal class GameServerTick
    {
        private string version;
        private DataContext dbContext;

        public GameServerTick()
        {
            this.version = this.GetType().Assembly.GetName().Version.ToString();
            this.dbContext = new DataContext(ServiceLocator.Current.GetInstance<IApplicationSettings>().DatabaseConnection);
        }

        public void Process()
        {
            this.FindDecidedMatchesAndFinishThem();

            this.KickInactivePlayers();

            this.AssignWaitingPlayersToNewGames();

            this.CreateNextRoundForActiveGames();
        }

        private void FindDecidedMatchesAndFinishThem()
        {
            var serializer = new GameFleetSerializer();

            var allActiveGames = this.dbContext.Matches.Where(m => m.State == MatchState.Started && m.Rounds.Any());

            foreach (var match in allActiveGames)
            {
                var lastRound = match.Rounds.Last();

                if (match.Rounds != null && !string.IsNullOrEmpty(lastRound.PlayerOneCell) && !string.IsNullOrEmpty(lastRound.PlayerTwoCell))
                {
                    match.CurrentPlayer = null;

                    var playerOneGameFleet = serializer.Deserialize(match.FirstPlayerFleet.RawFleetValue);
                    var playerTwoGameFleet = serializer.Deserialize(match.SecondPlayerFleet.RawFleetValue);

                    if (playerOneGameFleet.AllSunk || playerTwoGameFleet.AllSunk)
                    {
                        match.State = MatchState.Finished;

                        if (!playerOneGameFleet.AllSunk && playerTwoGameFleet.AllSunk)
                        {
                            // Player 1 wins
                            match.Winner = match.FirstPlayer;
                        }
                        else if (!playerTwoGameFleet.AllSunk && playerOneGameFleet.AllSunk)
                        {
                            // Player 1 wins
                            match.Winner = match.SecondPlayer;
                        }
                        else
                        {
                            // No one wins
                            match.Winner = null;
                        }
                    }
                }

            }

            this.dbContext.SaveChanges();

        }

        private void CreateNextRoundForActiveGames()
        {
            
        }

        private void AssignWaitingPlayersToNewGames()
        {
            
        }

        private void KickInactivePlayers()
        {
            
        }
    }
}