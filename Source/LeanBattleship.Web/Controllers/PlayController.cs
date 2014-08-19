using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Practices.ServiceLocation;

namespace LeanBattleship.Web.Controllers
{
    public class PlayController : ApiController
    {
        [HttpGet]
        [Route("api/game/{gameId}/state")]
        public IHttpActionResult GetStateForGame(string gameId)
        {
            var tournament = ServiceLocator.Current.GetInstance<ITournamentService>();

            var game = tournament.GetGame(gameId);

            if (game == null)
            {
                return this.NotFound();
            }

            return this.Ok();
        }

        [HttpPut]
        [Route("api/game/{gameId}/ships")]
        public IHttpActionResult SetupShipsForGame(string gameId, List<ShipPlacement> )
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
        [Route("api/game/{gameId}/fire/{position}")]
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
