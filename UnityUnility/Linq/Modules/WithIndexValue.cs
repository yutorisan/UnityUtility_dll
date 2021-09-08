using System;
namespace UnityUtility.Linq.Core
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

    
}
