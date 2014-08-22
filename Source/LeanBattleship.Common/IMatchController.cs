using LeanBattleship.Model;

namespace LeanBattleship.Common
{
    public interface IMatchController
    {
        bool Fire(Player player, string position);
    }
}