using System.Web.Http;

namespace LeanBattleship.Web.Controllers
{
    public class TournamentController : ApiController
    {
        [HttpGet]
        [Route("api/tournaments")]
        public IHttpActionResult GetAllTournaments()
        {
            return Json(new object[] { });
        }

        [HttpGet]
        //[Route("api/tournaments/{tournamentId}/join?name={playerName}&callbackurl={callbackurl}")]
        public IHttpActionResult JoinTournament(string tournamentId, string playerName, string callbacklUrl)
        {
            return this.Ok();
        }

        [HttpGet]
        [Route("api/tournaments/{tournamentId}/leave")]
        public IHttpActionResult LeaveTournament(string tournamentId)
        {
            return this.Ok();
        }
    }
}
