using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;

namespace LeanBattleship.Web.Controllers
{
    public class GameEventController : ApiController
    {
        private static readonly ConcurrentDictionary<string, ConcurrentQueue<StreamWriter>> AllClients = new ConcurrentDictionary<string, ConcurrentQueue<StreamWriter>>();


        [HttpGet]
        [Route("api/game/{gameId}/events")]
        public HttpResponseMessage GetEventStreamForGame(string gameId)
        {
            var response = this.Request.CreateResponse();
            response.Content = new PushStreamContent((stream, httpContent, transportContext) => this.StreamIsAvailable(gameId, stream, httpContent, transportContext), "text/event-stream");
            return response;
        }

        private void StreamIsAvailable(string gameId, Stream stream, HttpContent httpContent, TransportContext transportContext)
        {
            var streamwriter = new StreamWriter(stream);

            AllClients.AddOrUpdate(gameId, key => new ConcurrentQueue<StreamWriter>(new[] {streamwriter}),
                (s, queue) =>
                {
                    queue.Enqueue(streamwriter);
                    return queue;
                });
        }

        public static void SendEvent(string gameId, GameEvent m)
        {
            ConcurrentQueue<StreamWriter> registeredClients;

            if (AllClients.TryGetValue(gameId, out registeredClients))
            {
                foreach (var subscriber in registeredClients)
                {
                    subscriber.WriteLine("data:" + JsonConvert.SerializeObject(m) + "\n");
                    subscriber.Flush();
                }
            }

        }
    }

    public abstract class GameEvent
    {
    }
}
