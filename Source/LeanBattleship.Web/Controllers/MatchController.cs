using System.Web.Http;
using LeanBattleship.Common;
using Microsoft.Practices.ServiceLocation;

namespace LeanBattleship.Web.Controllers
{
    public class MatchController : ApiController
    {
        private readonly ITournamentService tournamentService;
        private readonly IPlayerService playerService;

        public MatchController()
        {
            this.tournamentService = ServiceLocator.Current.GetInstance<ITournamentService>();
            this.playerService = ServiceLocator.Current.GetInstance<IPlayerService>();
        }

        [HttpPut]
        [Route("api/match/{matchId}/setup")]
        public IHttpActionResult Setup(int matchId, [FromBody]string[][] ships)
        {
            var playerName = PlayerIdentifier.GetPlayerName(this.Request);
            var player = this.playerService.FindPlayer(playerName);

            if (player == null)
            {
                return this.BadRequest("Player not found");
            }

            var matchController = this.tournamentService.GetMatchController(matchId, player);

            if (matchController.SetShips(ships))
            {
                return this.Ok();
            }

            return this.BadRequest("Cannot set ships");
        }

        [HttpPost]
        [Route("api/match/{matchId}/fire/{position}")]
        public IHttpActionResult FireInGame(int matchId, string position)
        {
            var playerName = PlayerIdentifier.GetPlayerName(this.Request);
            var player = this.playerService.FindPlayer(playerName);

            if (player == null)
            {
                return this.BadRequest("Player not found");
            }

            var matchController = this.tournamentService.GetMatchController(matchId, player);



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
