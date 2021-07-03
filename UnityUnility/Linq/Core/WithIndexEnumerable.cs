using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityUtility.Linq.Core
{
    public interface IWithIndexEnumerable<T> : IEnumerable<WithIndexValue<T>>
    {

    }

    internal class WithIndexEnumerable<T> : IWithIndexEnumerable<T>
    {
        private readonly IEnumerable<T> source;

        public WithIndexEnumerable(IEnumerable<T> source) => this.source = source;

        public IEnumerator<WithIndexValue<T>> GetEnumerator()
        {
            int index = 0;
            foreach (var item in source)
            {
                yield return new WithIndexValue<T>(item, index++);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
