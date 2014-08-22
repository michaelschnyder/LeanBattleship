using System.Linq;
using System.Net.Http;

namespace LeanBattleship.Web
{
    public class PlayerIdentifier
    {
        public static string GetPlayerName(HttpRequestMessage requestMessage)
        {
            // Get Playername from Header
            var playerNameHeader = requestMessage.Headers.FirstOrDefault(h => h.Key.ToLowerInvariant() == "leanbattleship-player");
            return playerNameHeader.Value.First();
        }
    }
}