using System.ComponentModel;

namespace UnityUtility.Enums
{
    /// <summary>
    /// 上下左右
    /// </summary>
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public static class EnumExtensions
    {
        public static Direction ToDirection(this LeftRight lr) =>
            lr switch
            {
                LeftRight.Left => Direction.Left,
                LeftRight.Right => Direction.Right,
                _ => throw new InvalidEnumArgumentException()
            };
        public static Direction ToDirection(this UpDown ud) =>
            ud switch
            {
                UpDown.Up => Direction.Up,
                UpDown.Down => Direction.Down,
                _ => throw new InvalidEnumArgumentException()
            };
    }
}
