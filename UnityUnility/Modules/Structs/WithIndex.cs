using System;
namespace UnityUtility.Modules
{
    /// <summary>
    /// 値とインデックス番号のペア
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public readonly struct WithIndex<T> : IEquatable<WithIndex<T>>
    {
        public WithIndex(T value, int index) => (Value, Index) = (value, index);
        public T Value { get; }
        public int Index { get; }
        //分解メソッド
        public void Deconstruct(out T value, out int index) => (value, index) = (Value, Index);

        public bool Equals(WithIndex<T> other) =>
            this.Value.Equals(other.Value) &&
            this.Index == other.Index;
    }

    /// <summary>
    /// 値とインデックス番号のペア(long版)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public readonly struct WithIndexL<T> : IEquatable<WithIndexL<T>>
    {
        public WithIndexL(T value, long index) => (Value, Index) = (value, index);
        public T Value { get; }
        public long Index { get; }
        //分解メソッド
        public void Deconstruct(out T value, out long index) => (value, index) = (Value, Index);

        public bool Equals(WithIndexL<T> other) =>
            this.Value.Equals(other.Value) &&
            this.Index == other.Index;
    }
}
