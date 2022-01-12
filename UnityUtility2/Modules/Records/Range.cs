using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityUtility.Modules
{
    public record Range : IEnumerable<Cell>
    {
        public Range(Cell topleft, Cell bottomright)
        {

        }

        public IEnumerator<Cell> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}

