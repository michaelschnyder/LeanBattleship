﻿using System.Collections.Generic;
using LeanBattleship.Model;

namespace LeanBattleship.Common
{
    public interface ITournamentService
    {
        bool Exists(int tournamentId);
        Tournament FindById(int tournamentId);
        void AddPlayer(Tournament tournament, Player player);
        List<Tournament> GetAll();
        void RemovePlayer(Tournament tournament, Player player);
        IMatchServiceController GetMatchController(int matchId, Player player);
        Tournament Create(string name);
    }
}