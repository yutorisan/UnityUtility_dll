using System;
namespace UnityUtility.Modules
{
    public interface ICell<out T>
    {
        int Column { get; }
        int Row { get; }
        T Value { get; }
    }
    public record Cell<T> : ICell<T>
    {
        public Cell(int column, int row, T value)
        {
            if (column < 0 || row < 0) throw new ArgumentOutOfRangeException("マイナスのセルは作成できません");

            (Column, Row, Value) = (column, row, value);
        }

        public int Column { get; }
        public int Row { get; }
        public T Value { get; }

        public void Deconstruct(out int column, out int row, out T value) => (column, row, value) = (Column, Row, Value);
    }
}

