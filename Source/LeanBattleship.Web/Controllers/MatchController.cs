using System.Collections.Generic;
using System.Web.Http;
using Microsoft.Practices.ServiceLocation;

namespace LeanBattleship.Web.Controllers
{
    public class MatchController : ApiController
    {
        [HttpGet]
        [Route("api/match/{matchId}/state")]
        public IHttpActionResult GetStateForGame(string matchId)
        {
            var tournament = ServiceLocator.Current.GetInstance<ITournamentService>();

            var game = tournament.GetGame(matchId);

            if (game == null)
            {
                return this.NotFound();
            }

            return this.Ok();
        }

        [HttpPut]
        [Route("api/match/{matchId}/setup")]
        public IHttpActionResult SetupShipsForGame(string gameId, List<ShipPlacement> shipPlacements)
        {
            var tournament = ServiceLocator.Current.GetInstance<ITournamentService>();

            var game = tournament.GetGame(gameId);

            if (game == null)
            {
                return this.NotFound();
            }

            return this.Ok();
        }

        [HttpPost]
        [Route("api/match/{matchId}/fire/{position}")]
        public IHttpActionResult FireInGame(string gameId, string position)
        {
            var tournament = ServiceLocator.Current.GetInstance<ITournamentService>();

            var game = tournament.GetGame(gameId);

            if (game == null)
            {
                return this.NotFound();
            }

            if (game.Fire())
            {
                return this.Ok();
            }

            return this.Ok();
        }
    }

    public class ShipPlacement
    {
        private List<KeyValuePair<string, string>> positionsList = new List<KeyValuePair<string, string>>();
    }

    public interface ITournamentService
    {
        BattleShipGame GetGame(string gameId);
    }

    public class BattleShipGame
    {
        public bool Fire()
        {
            return true;
        }
    }
}
