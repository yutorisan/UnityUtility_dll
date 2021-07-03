using System;
namespace UnityUtility.Linq.Core
{
    /// <summary>
    /// 値とインデックス番号のペア
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public readonly struct WithIndexValue<T> : IEquatable<WithIndexValue<T>>
    {
        public WithIndexValue(T value, int index) => (Value, Index) = (value, index);
        public T Value { get; }
        public int Index { get; }
        //分解メソッド
        public void Deconstruct(out T value, out int index) => (value, index) = (Value, Index);

        public bool Equals(WithIndexValue<T> other) =>
            this.Value.Equals(other.Value) &&
            this.Index == other.Index;
    }
}
