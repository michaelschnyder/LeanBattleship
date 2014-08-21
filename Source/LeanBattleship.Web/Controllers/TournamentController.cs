using System;
using System.Linq;
using System.Web.Http;
using LeanBattleship.Common;
using LeanBattleship.Core;
using LeanBattleship.Web.Dto;
using Microsoft.Practices.ServiceLocation;

namespace LeanBattleship.Web.Controllers
{
    public class TournamentController : ApiController
    {
        private readonly ITournamentService tournamentService;
        private readonly IPlayerService playerService;

        public TournamentController()
        {
            this.tournamentService = ServiceLocator.Current.GetInstance<ITournamentService>();
            this.playerService = ServiceLocator.Current.GetInstance<IPlayerService>();
        }

        [HttpGet]
        [Route("api/tournaments")]
        public IHttpActionResult GetAllTournaments()
        {
            var allTournaments = this.tournamentService.GetAll();

            var dtos = allTournaments.Select(t => new TournametDto {Id = t.Id, Name = t.Name});
            return Json(dtos);
        }

        [HttpPost]
        [Route("api/tournaments")]
        public IHttpActionResult AddTournament([FromBody]TournametDto tournamentDto)
        {
            var created = this.tournamentService.Create(tournamentDto.Name);

            if (created != null)
            {
                return this.Created(new Uri(this.Request.RequestUri + "/" + created.Id), new TournametDto() { Id = created.Id, Name = created.Name });
            }

            return this.BadRequest();
        }

        [HttpGet]
        [Route("api/tournaments/{tournamentId}/join")]
        public IHttpActionResult JoinTournament(int tournamentId, string playerName)
        {
            if (!this.tournamentService.Exists(tournamentId))
            {
                return this.NotFound();
            }

            var player = this.playerService.FindPlayer(playerName);

            if (player == null)
            {
                player = this.playerService.CreatePlayer(playerName);
            }

            // Do it really
            var tournament = this.tournamentService.FindById(tournamentId);
            this.tournamentService.AddPlayer(tournament, player);

            return this.Ok();
        }

        [HttpGet]
        [Route("api/tournaments/{tournamentId}/leave")]
        public IHttpActionResult LeaveTournament(int tournamentId)
        {
            if (!this.tournamentService.Exists(tournamentId))
            {
                return this.NotFound();
            }

            var playerName = PlayerIdentifier.GetPlayerName(this.Request);
            var player = this.playerService.FindPlayer(playerName);

            if (player == null)
            {
                return this.BadRequest("Player not found");
            }

            // Do it really.
            var tournament = this.tournamentService.FindById(tournamentId);
            this.tournamentService.RemovePlayer(tournament, player);

            return this.Ok();
        }
    }
}
