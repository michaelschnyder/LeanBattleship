using System.Collections.Generic;
using LeanBattleship.Core.Game;

namespace LeanBattleship.Common
{
    public interface IMatchController
    {
        bool Fire(string position);
        bool SetShips(List<Ship> ships);
    }
}