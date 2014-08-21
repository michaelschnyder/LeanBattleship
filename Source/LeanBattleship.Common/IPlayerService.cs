using System.Collections.Generic;
using LeanBattleship.Model;

namespace LeanBattleship.Common
{
    public interface IPlayerService
    {
        Player FindPlayer(string playerName);
        Player CreatePlayer(string playerName);
        List<Match> GetActiveMatches(Player player);
    }
}