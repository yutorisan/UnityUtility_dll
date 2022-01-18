using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using UnityUtility.Collections.Core;
using UnityUtility.Enums;
using UnityUtility.Modules;

namespace UnityUtility.Collections
{
    /// <summary>
    /// 二次元マトリックス構造
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MapSlim<T> : Map<T>, IFixedMapSlim<T>, IMap<T> where T : unmanaged
    {
        public MapSlim(int column, int row, T plainValue = default) : base(column, row, plainValue)
        {
        }

        public MapSlim(int column, int row, T[] source1dArray, T plainValue = default) : base(column, row, source1dArray, plainValue)
        {
        }

        public MapSlim(T[,] source2dArray, T plainValue = default) : base(source2dArray, plainValue)
        {
        }

        public unsafe Span<T> GetRowSpan(int row)
        {
            fixed(T* pointer = &m_map[row * ColumnCount])
            {
                return new Span<T>(pointer, ColumnCount);
            }
        }

        public unsafe Span<T> GetAllSpan()
        {
            fixed(T* pointer = m_map)
            {
                return new Span<T>(pointer, m_map.Length);
            }
        }
    }
}

