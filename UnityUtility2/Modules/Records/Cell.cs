using System;
namespace UnityUtility.Modules
{
    public record Cell
    {
        public Cell(int column, int row)
        {
            if (column < 0 || row < 0) throw new ArgumentOutOfRangeException("マイナスのセルは作成できません");

            (Column, Row) = (column, row);
        }

        public int Column { get; init; }
        public int Row { get; init; }
    }
}

