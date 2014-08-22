using System;
using System.Threading;

namespace LeanBattleship.Core.Game
{
    public class GameServer
    {
        private Timer processMatchesTimer = new Timer(Callback);
        private bool isStarted;

        public GameServer()
        {
        }

        public void Start()
        {
            if (!this.isStarted)
            {
                this.isStarted = true;
                this.processMatchesTimer.Change(TimeSpan.FromMilliseconds(10), TimeSpan.FromMilliseconds(500));
            }
        }

        public void Stop()
        {
            if (this.isStarted)
            {
                this.processMatchesTimer.Change(TimeSpan.FromMilliseconds(10), TimeSpan.FromMilliseconds(500));
                this.isStarted = false;
            }
        }

        private static void Callback(object state)
        {
            var tick = new GameServerTick();
            tick.Process();
        }
    }
}
