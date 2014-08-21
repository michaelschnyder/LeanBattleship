using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using LeanBattleship.Common;
using LeanBattleship.Data;
using LeanBattleship.Model;
using Microsoft.Practices.ServiceLocation;

namespace LeanBattleship.Core
{
    public class GameServer
    {
        private Timer processMatchesTimer = new Timer(Callback);
        private bool isStarted;

        public GameServer()
        {
        }

        public void Start()
        {
            if (!this.isStarted)
            {
                this.isStarted = true;
                this.processMatchesTimer.Change(TimeSpan.FromMilliseconds(10), TimeSpan.FromMilliseconds(500));
            }
        }

        public void Stop()
        {
            if (this.isStarted)
            {
                this.processMatchesTimer.Change(TimeSpan.FromMilliseconds(10), TimeSpan.FromMilliseconds(500));
                this.isStarted = false;
            }
        }

        private static void Callback(object state)
        {
            var tick = new GameServerTick();
            tick.Process();
        }
    }

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
            var allActiveGames = dbContext.Matches.Where(m => m.State == MatchState.Started && m.Rounds.Any());

            foreach (var match in allActiveGames)
            {
                var lastRound = match.Rounds.Last();

                if (match.Rounds != null && !string.IsNullOrEmpty(lastRound.PlayerOneCell) && !string.IsNullOrEmpty(lastRound.PlayerTwoCell))
                {
                    match.CurrentPlayer = null;

                    var playerOneGameFleet = new GameFleet(match.FirstPlayerFleet.RawFleetValue);
                    var playerTwoGameFleet = new GameFleet(match.SecondPlayerFleet.RawFleetValue);

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

    internal class GameFleet
    {
        public GameFleet(string rawFleetValue)
        {
            
        }

        public string RawValue
        {
            get { return string.Empty; }
        }

        public bool AllSunk { get; private set; }

        public bool Fire(string field)
        {
            return false;
        }
    }
}
