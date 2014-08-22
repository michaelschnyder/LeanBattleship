using System.Collections.Generic;
using System.Linq;

namespace LeanBattleship.Core.Game
{
    public class Ship
    {
        private readonly List<Cell> initializedCells;

        public Ship(Cell cell)
        {
            this.initializedCells = new List<Cell>(new[] { cell});
        }

        public int Size
        {
            get { return this.initializedCells.Count; }
        }

        public IEnumerable<Cell> InitializedCells
        {
            get { return this.initializedCells; }
        }

        public IEnumerable<Cell> BombedCells
        {
            get { return this.initializedCells.Where(c => c.State == CellState.Hit || c.State == CellState.Destroyed); }
        }

        public bool IsDestroyed
        {
            get { return this.initializedCells.TrueForAll(c => c.State == CellState.Hit || c.State == CellState.Destroyed); }
        }

        public bool HasBeenHit()
        {
            { return this.initializedCells.Any(c => c.State == CellState.Hit || c.State == CellState.Destroyed); }
        }

        public void AddCell(Cell cell)
        {
            this.initializedCells.Add(cell);
            cell.Ship = this;
        }

        /*
        public BombardmentResult Bomb(Cell cell)
        {
            if (!this.InitializedCells.Contains(cell))
            {
                return BombardmentResult.Water; // missed the ship.
            }

            if (this.IsDestroyed)
            {
                return BombardmentResult.Water; // we are already down.
            }

            if (!this.bombedCells.Contains(cell))
            {
                this.bombedCells.Add(cell);
            }

            return IsDestroyed ? BombardmentResult.Destroyed : BombardmentResult.Hit;
        }
        */
        /*
        /// <summary>
        /// Two ships are in conflict if they touch.
        /// (What does 'touch' mean?!)
        /// (This code could be simplified without diagonal entries)
        /// </summary>
        public bool IsInConflictWith(Ship otherShip)
        {
            Cell[] blockedCells = this.GetBlockedCells().ToArray();
            Cell[] otherShipCells = otherShip.InitializedCells.ToArray();

            return otherShipCells.Any(blockedCells.Contains);
        }
        */
    }
}