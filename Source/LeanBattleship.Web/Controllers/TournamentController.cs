using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using LeanBattleship.Common;
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
        [Route("api/tournament")]
        public IHttpActionResult GetAllTournaments()
        {
            var allTournaments = this.tournamentService.GetAll();

            var dtos = allTournaments.Select(t => new TournametDto {Id = t.Id, Name = t.Name});
            return Json(dtos);
        }

        [HttpPost]
        [Route("api/tournament")]
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
        [Route("api/tournament/{tournamentId}/join")]
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
        [Route("api/tournament/{tournamentId}/mymatches")]
        public IHttpActionResult MyMatchesInTournament(int tournamentId)
        {
            var myMatches = new List<MyMatchesDto>();

            var playerName = PlayerIdentifier.GetPlayerName(this.Request);
            var player = this.playerService.FindPlayer(playerName);

            if (playerName == null)
            {
                return this.BadRequest("Player not found");
            }

            var matchesFromDb = this.playerService.GetActiveMatches(player);

            foreach (var match in matchesFromDb)
            {
                var myMatch = new MyMatchesDto
                {
                    MatchId = match.Id,
                    TournamentId = tournamentId,
                    MatchState = match.State.ToString()
                };

                if (match.CurrentPlayer == player)
                {
                    myMatch.WaitingFor = "You";
                }
                else if (match.CurrentPlayer != null)
                {
                    myMatch.WaitingFor = "Other";
                }
                else
                {
                    myMatch.WaitingFor = "Server";
                }
            }

            return Json(myMatches);
        }

        [HttpGet]
        [Route("api/tournament/{tournamentId}/leave")]
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
