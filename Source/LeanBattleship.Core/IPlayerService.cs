using LeanBattleship.Model;

namespace LeanBattleship.Web.Controllers
{
    public interface IPlayerService
    {
        Player FindPlayer(string playerName);
        Player CreatePlayer(string playerName);
    }
}