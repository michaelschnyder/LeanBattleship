using System;
using System.Threading;

namespace LeanBattleship.Core.Game
{
    public class GameServer
    {
        private Timer processMatchesTimer;
        private bool isStarted;
        private object singleProcessLock = new object();
        private bool isRunning = false;

        public GameServer()
        {
             this.processMatchesTimer = new Timer(this.Callback);
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

        private void Callback(object state)
        {
            if (!this.isRunning)
            {
                lock (this.singleProcessLock)
                {
                    if (!this.isRunning)
                    {
                        this.isRunning = true;
                        var tick = new GameServerTick();
                        tick.Process();
                        this.isRunning = false;
                    }
                }
            }
        }
    }
}
