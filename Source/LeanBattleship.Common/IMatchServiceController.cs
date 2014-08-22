using System.Collections.Generic;

namespace LeanBattleship.Common
{
    public interface IMatchServiceController
    {
        bool Fire(string position);
        bool SetShips(string[][] ships);
    }
}