using System.Collections.Generic;
using LeanBattleship.Model;

namespace LeanBattleship.Core
{
    public interface ITournamentService
    {
        bool Exists(int tournamentId);
        Tournament FindById(int tournamentId);
        void AddPlayer(Tournament tournament, Player player);
        List<Tournament> GetAll();
        void RemovePlayer(Tournament tournament, Player player);
        IMatchService GetMatchController(string matchId);
    }

    public interface IMatchService
    {
    }
}