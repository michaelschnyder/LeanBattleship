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
    public class TournamentService : ITournamentService
    {
        private DataContext dbContext;

        public TournamentService()
        {
            this.dbContext = ServiceLocator.Current.GetInstance<DataContext>();
        }

        public bool Exists(int tournamentId)
        {
            return this.dbContext.Tournaments.Find(tournamentId) != null;
        }

        public Tournament FindById(int tournamentId)
        {
            return this.dbContext.Tournaments.FirstOrDefault(t => t.Id == tournamentId);
        }

        public List<Tournament> GetAll()
        {
            return this.dbContext.Tournaments.ToList();
        }

        public IMatchController GetMatchController(int matchId, Player player)
        {
            return new MatchController(matchId, player, this.dbContext);
        }

        public Tournament Create(string name)
        {
            var tournament = new Tournament() {Name = name};
            
            this.dbContext.Tournaments.Add(tournament);

            this.dbContext.SaveChanges();

            return tournament;
        }

        public void RemovePlayer(Tournament tournament, Player player)
        {
            var t = this.dbContext.Tournaments.Find(tournament.Id);
            
            if (t.Players.Contains(player))
            {
                t.Players.Remove(player);
            }

            this.dbContext.SaveChanges();
        }

        public void AddPlayer(Tournament tournament, Player player)
        {
            var t = this.dbContext.Tournaments.Where(tour => tour.Id == tournament.Id).Include("Players").First();

            if (!t.Players.Contains(player))
            {
                t.Players.Add(player);
            }

            this.dbContext.SaveChanges();
        }
    }
}
