using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using LeanBattleship.Common;
using LeanBattleship.Core.Game;
using LeanBattleship.Data;
using LeanBattleship.Model;

namespace LeanBattleship.Core.Services
{
    class MatchServiceServiceController : IMatchServiceController
    {
        private readonly int matchId;
        private readonly DataContext context;
        private Player player;

        public MatchServiceServiceController(int matchId, Player player, DataContext context)
        {
            this.matchId = matchId;
            this.context = context;

            this.player = player;
        }

        public bool SetShips(string[][] ships)
        {
            var match = this.GetMatch();

            if (this.IsFirstPlayer(match) && !string.IsNullOrEmpty(match.FirstPlayerFleetRaw)) return false;
            if (!this.IsFirstPlayer(match) && !string.IsNullOrEmpty(match.SecondPlayerFleetRaw)) return false;

            var shipsList = this.GetShips(ships);

            var fleet = new GameFleet(10, shipsList);
            var serializer = new GameFleetSerializer();

            if (this.IsFirstPlayer(match))
            {
                match.FirstPlayerFleetRaw = serializer.Serialize(fleet);
            }
            else
            {
                match.SecondPlayerFleetRaw = serializer.Serialize(fleet);
            }

            this.context.SaveChanges();
            return true;
        }

        private Match GetMatch()
        {
            var all =  this.context.Matches.Where(m => m.Id ==this.matchId).Include("CurrentPlayer").Include("FirstPlayer").Include("SecondPlayer").ToList();
            return all.First();
        }

        public bool Fire(string position)
        {
            var serializer = new GameFleetSerializer();

            // Convert to x/y
            var colString = position.ToLower()[0];

            var colValue = colString - 97;
            var rowValue = int.Parse(position[1].ToString()) - 1;

            var match = this.GetMatch();
            var wasHit = false;

            if (match.CurrentPlayer == this.player)
            {
                if (this.IsFirstPlayer(match))
                {
                    var oppositePlayerFleet = serializer.Deserialize(match.SecondPlayerFleetRaw);
                    wasHit = oppositePlayerFleet.Fire(rowValue, colValue);
                    match.SecondPlayerFleetRaw = serializer.Serialize(oppositePlayerFleet);

                    if (!wasHit)
                    {
                        match.CurrentPlayer = match.SecondPlayer;
                    }
                }
                else
                {
                    var oppositePlayerFleet = serializer.Deserialize(match.FirstPlayerFleetRaw);
                    wasHit = oppositePlayerFleet.Fire(rowValue, colValue);
                    match.FirstPlayerFleetRaw = serializer.Serialize(oppositePlayerFleet);

                    if (!wasHit)
                    {
                        match.CurrentPlayer = null;
                    }
                }
            }

            return wasHit;
        }

        private List<Ship> GetShips(IEnumerable<string[]> ships)
        {
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

            return shipsToAdd;
        }

        private Cell ConvertToCell(string position)
        {
            var colString = position.ToLower()[0];

            var colValue = colString - 97;
            var rowValue = int.Parse(position[1].ToString()) - 1;

            return new Cell() { Col = colValue, Row = rowValue };
        }

        private bool IsFirstPlayer(Match match)
        {
            return this.player == match.FirstPlayer;
        }
    }
}