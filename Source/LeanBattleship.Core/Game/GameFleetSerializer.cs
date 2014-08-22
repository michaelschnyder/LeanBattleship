using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeanBattleship.Core.Game
{
    internal class GameFleetSerializer
    {
        public GameFleet Deserialize(string rawFleetValue)
        {
            var rows = rawFleetValue.Split(new []{ '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var maxColumns = rows.Max(r => r.Length);

            var allCells = new List<Cell>();
            
            for (int i = 0; i < rows.Count(); i++)
            {
                var cells = rows[i].ToArray();

                var shipRow = new List<char>(maxColumns);

                int column = 0;
                foreach (var c in cells)
                {
                    var cell = new Cell() { Row = i, Col = column, State = this.ConvertToState(c) };
                    allCells.Add(cell);
                    column++;
                }

                shipRow.AddRange(cells);
            }

            var fleet = new GameFleet(allCells);
            return fleet;
        }

        private CellState ConvertToState(char cellValue)
        {
            if (cellValue == 'X') return CellState.Hit;
            if (cellValue == '-') return CellState.Destroyed;

            return CellState.Water;
        }

        private char ConvertToValue(CellState cellState)
        {
            if (cellState == CellState.Hit) return 'X';
            if (cellState == CellState.Destroyed) return '-';

            return '0';
        }

        public string Serialize(GameFleet fleet)
        {
            var cells = fleet.Cells;
            var sb = new StringBuilder();

            var rowsGroup = cells.GroupBy(c => c.Row);

            foreach (var row in rowsGroup.Select(g => g.ToList()))
            {
                foreach (var cell in row)
                {
                    sb.Append(this.ConvertToValue(cell.State));
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }
    }

    public enum CellState
    {
        Water,

        Ship,
        Hit,
        Destroyed,
    }
}