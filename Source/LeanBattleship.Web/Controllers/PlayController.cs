using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LeanBattleship.Web.Controllers
{
    public class PlayController : ApiController
    {
        [HttpGet]
        [Route("api/game/{gameId}/state")]
        public IHttpActionResult GetStateForGame(string gameId)
        {
            return this.Ok();
        }

        [HttpPost]
        [Route("api/game/{gameId}/setup")]
        public IHttpActionResult SetupShipsForGame(string gameId)
        {
            return this.Ok();
        }

        [HttpPost]
        [Route("api/game/{gameId}/fire/{position}")]
        public IHttpActionResult FireInGame(string gameId, string position)
        {
            return this.Ok();
        }
    }
}
