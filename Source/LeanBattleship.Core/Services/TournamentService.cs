using System;
using System.Collections.Generic;
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
            this.dbContext = new DataContext(ServiceLocator.Current.GetInstance<IApplicationSettings>().DatabaseConnection);
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

        public IMatchController GetMatchController(string matchId)
        {
            throw new NotImplementedException();
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
            
        }

        public void AddPlayer(Tournament tournament, Player player)
        {
            throw new NotImplementedException();
        }
    }
}
