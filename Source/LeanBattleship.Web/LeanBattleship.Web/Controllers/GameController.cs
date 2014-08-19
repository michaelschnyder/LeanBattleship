using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LeanBattleship.Web.Controllers
{
    public class GameController : ApiController
    {
        [HttpGet]
        [Route("api/games")]
        public IHttpActionResult GetAllGames()
        {
            return Json(new object[] { });
        }

        [HttpGet]
        [Route("api/game/{gameId}/join")]
        public IHttpActionResult JoinAGame(string gameId)
        {
            return this.Ok();
        }

        [HttpGet]
        [Route("api/game/{gameId}/leave")]
        public IHttpActionResult LeaveAGame(string gameId)
        {
            return this.Ok();
        }
    }
}
