using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
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
            this.dbContext = ServiceLocator.Current.GetInstance<DataContext>();
        }

        public void Process()
        {
            {
                this.FindDecidedMatchesAndFinishThem();

                this.KickInactivePlayers();

                this.AssignWaitingPlayersToNewGames();

                this.CreateFirstRoundForSetupGames();

                this.CreateNextRoundForActiveGames();

            }
        }

        private void FindDecidedMatchesAndFinishThem()
        {
            var serializer = new GameFleetSerializer();

            var allActiveGames = this.dbContext.Matches.Where(m => m.State == MatchState.Started).ToList();

            foreach (var match in allActiveGames)
            {
                var lastRound = match.Rounds.Last();

                if (match.Rounds != null && !string.IsNullOrEmpty(lastRound.PlayerOneCell) && !string.IsNullOrEmpty(lastRound.PlayerTwoCell))
                {
                    match.CurrentPlayer = null;

                    var playerOneGameFleet = serializer.Deserialize(match.FirstPlayerFleetRaw);
                    var playerTwoGameFleet = serializer.Deserialize(match.SecondPlayerFleetRaw);

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

        private void CreateFirstRoundForSetupGames()
        {
            var allSetupGames = this.dbContext.Matches.Where(m => m.State == MatchState.Setup).ToList();

            foreach (var match in allSetupGames)
            {
                if (!string.IsNullOrEmpty(match.FirstPlayerFleetRaw) && !string.IsNullOrEmpty(match.SecondPlayerFleetRaw))
                {
                    // Match is ready
                    match.State = MatchState.Started;
                    match.StartTimeUtc = DateTime.UtcNow;
                    match.CurrentPlayer = match.CurrentPlayer;
                }
            }

            this.dbContext.SaveChanges();
        }


        private void CreateNextRoundForActiveGames()
        {
            
        }

        private void AssignWaitingPlayersToNewGames()
        {
            var allActiveGames = this.dbContext.Matches.Where(m => m.State != MatchState.Finished);

            var activePlayers = new List<Player>();
            activePlayers.AddRange(allActiveGames.Select(m => m.FirstPlayer));
            activePlayers.AddRange(allActiveGames.Select(m => m.SecondPlayer));

            var allTournaments = this.dbContext.Tournaments.Include("Players").Include("Matches").ToList();

            foreach (var tournament in allTournaments)
            {
                var unAssignedPlayers = tournament.Players.Where(tournamentPlayer => !activePlayers.Contains(tournamentPlayer)).ToList();

                while (unAssignedPlayers.Count >= 2)
                {
                    var player1 = unAssignedPlayers[new Random().Next(0, unAssignedPlayers.Count - 1)];
                    unAssignedPlayers.Remove(player1);

                    var player2 = unAssignedPlayers[new Random().Next(0, unAssignedPlayers.Count - 1)];
                    unAssignedPlayers.Remove(player2);

                    var match = new Match()
                    {
                        FirstPlayer = player1,
                        SecondPlayer = player2,
                        SetupTimeUtc = DateTime.UtcNow,
                        State = MatchState.Setup,
                    };

                    this.dbContext.Matches.Add(match);
                    this.dbContext.SaveChanges();

                    tournament.Matches.Add(match);
                    this.dbContext.SaveChanges();
                }
            }

            this.dbContext.SaveChanges();
        }

        private void KickInactivePlayers()
        {
            
        }
    }
}