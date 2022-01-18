using System;

namespace UnityUtility.Collections
{
    public interface IFixedMapSlim<T> : IFixedMap<T> where T : unmanaged
    {
        Span<T> GetRowSpan(int row);
        Span<T> GetAllSpan();
    }
}

