using System;
using System.Collections.Generic;
using System.Net.Http.Formatting;
using System.Web.Http;
using LeanBattleship.Common;
using LeanBattleship.Core.Game;
using Microsoft.Practices.ObjectBuilder2;
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

        [HttpGet]
        [Route("api/match/{matchId}/state")]
        public IHttpActionResult GetStateForGame(int matchId)
        {
            var match = this.tournamentService.GetMatchController(matchId, null);

            if (match == null)
            {
                return this.NotFound();
            }

            return this.Ok();
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

            var shipsToAdd = new List<Ship>();

            foreach (string[] shipCellStrings in ships)
            {
                Ship currentShip = null;

                foreach (var position in shipCellStrings)
                {
                    var cell = this.ConvertToCell(position);
                    cell.State = CellState.Ship;

                    if (currentShip == null)
                    {
                        currentShip = new Ship(cell);
                    }
                    else
                    {
                        currentShip.AddCell(cell);
                    }
                }

                if (currentShip != null)
                {
                    shipsToAdd.Add(currentShip);
                }
            }

            if (matchController.SetShips(shipsToAdd))
            {
                return this.Ok();
            }

            return this.BadRequest("Cannot set ships");
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

        private Cell ConvertToCell(string position)
        {
            var colString = position.ToLower()[0];

            var colValue = colString - 97;
            var rowValue = int.Parse(position[1].ToString()) - 1;

            return new Cell() { Col = colValue, Row = rowValue };
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
