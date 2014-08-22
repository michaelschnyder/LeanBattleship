namespace LeanBattleship.Core.Game
{
    public class Cell
    {
        public int Row { get; set; }

        public int Col { get; set; }

        public CellState State { get; set; }

        public Ship Ship { get; set; }
    }
}