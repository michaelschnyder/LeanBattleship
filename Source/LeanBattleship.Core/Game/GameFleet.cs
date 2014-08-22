using System;
using System.Collections.Generic;
using System.Linq;

namespace LeanBattleship.Core.Game
{
    internal class GameFleet
    {
        private readonly List<Cell> cells;
        private List<Ship> ships;

        public List<Cell> Cells
        {
            get { return this.cells; }
        }

        public GameFleet(List<Cell> cells)
        {
            var numRows = cells.Max(c => c.Row);
            var numCols = cells.Max(c => c.Col);

            if (numCols != numRows)
            {
                throw new Exception("Not equal size of rows and cols");
            }

            if (numCols*numRows != cells.Count)
            {
                throw new Exception("Not a col for every field");
            }

            this.cells = cells;

            this.DetectShips();
        }

        private void DetectShips()
        {
            this.ships = new List<Ship>();

            this.ships.AddRange(FindShips(this.Cells.GroupBy(c => c.Row)));
            this.ships.AddRange(FindShips(this.Cells.GroupBy(c => c.Col)));
        }

        private static IEnumerable<Ship> FindShips(IEnumerable<IGrouping<int, Cell>> spliceGroup)
        {
            var ships = new List<Ship>();

            foreach (var splice in spliceGroup.Select(r => r.ToList()))
            {
                Ship currentShip = null;
                for (int i = 0; i < splice.Count(); i++)
                {
                    if (splice[i].State != CellState.Water)
                    {
                        if (currentShip == null)
                        {
                            currentShip = new Ship(splice[i]);
                            ships.Add(currentShip);
                        }
                        else
                        {
                            currentShip.AddCell(splice[i]);
                        }

                        splice[i].Ship = currentShip;
                    }
                    else
                    {
                        currentShip = null;
                    }
                }
            }
            return ships;
        }

        public string RawValue
        {
            get { return string.Empty; }
        }

        public bool AllSunk
        {
            get { return this.ships.TrueForAll(s => s.IsDestroyed); }
        }

        public bool Fire(int row, int col)
        {
            var cell = this.Cells.FirstOrDefault(c => c.Row == row && c.Col == col);

            if (cell != null && cell.Ship != null)
            {
                cell.State = CellState.Hit;
                return true;
            }

            return false;
        }
    }
}