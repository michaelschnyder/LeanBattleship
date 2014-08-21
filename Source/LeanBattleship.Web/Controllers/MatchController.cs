using System.Collections.Generic;
using System.Web.Http;
using LeanBattleship.Core;
using Microsoft.Practices.ServiceLocation;

namespace LeanBattleship.Web.Controllers
{
    public class MatchController : ApiController
    {
        private readonly ITournamentService tournamentService;

        public MatchController()
        {
            this.tournamentService = ServiceLocator.Current.GetInstance<ITournamentService>();
        }

        [HttpGet]
        [Route("api/match/{matchId}/state")]
        public IHttpActionResult GetStateForGame(string matchId)
        {
            var game = this.tournamentService.GetMatchController(matchId);

            if (game == null)
            {
                return this.NotFound();
            }

            return this.Ok();
        }

        [HttpPut]
        [Route("api/match/{matchId}/setup")]
        public IHttpActionResult SetupShipsForGame(string gameId, List<ShipPlacementDto> shipPlacements)
        {
            var tournament = ServiceLocator.Current.GetInstance<ITournamentService>();

            /*
            var game = tournament.GetGame(gameId);

            if (game == null)
            {
                return this.NotFound();
            }
            */
            return this.Ok();
        }

        [HttpPost]
        [Route("api/match/{matchId}/fire/{position}")]
        public IHttpActionResult FireInGame(string gameId, string position)
        {
            var tournament = ServiceLocator.Current.GetInstance<ITournamentService>();
            /*
            var game = tournament.GetGame(gameId);

            if (game == null)
            {
                return this.NotFound();
            }

            if (game.Fire())
            {
                return this.Ok();
            }
            */
            return this.Ok();
        }
    }

    
    public class ShipPlacementDto
    {
        public string[] Cells { get; set; }
    }

    /*
    public class BattleShipGame
    {
        public bool Fire()
        {
            return true;
        }
    }
    */
}
