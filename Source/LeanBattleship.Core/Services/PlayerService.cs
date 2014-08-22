using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using LeanBattleship.Common;
using LeanBattleship.Data;
using LeanBattleship.Model;
using Microsoft.Practices.ServiceLocation;

namespace LeanBattleship.Core.Services
{
    public class PlayerService : IPlayerService
    {
        private DataContext dbContext;

        public PlayerService()
        {
            this.dbContext = ServiceLocator.Current.GetInstance<DataContext>();
        }

        public Player FindPlayer(string playerName)
        {
            return this.dbContext.Players.ToList().FirstOrDefault(p => p.Name == playerName);
        }

        public Player CreatePlayer(string playerName)
        {
            var player = new Player() {Name = playerName};
            this.dbContext.Players.Add(player);
            this.dbContext.SaveChanges();
            return player;
        }

        public List<Match> GetActiveMatches(Player player)
        {
            var matchesForPlayer = this.dbContext.Matches.Where(m => m.State != MatchState.Finished).Include("FirstPlayer").Include("SecondPlayer").ToList();

            return matchesForPlayer.Where(m => m.FirstPlayer == player || m.SecondPlayer == player).ToList();
        }
    }
}
