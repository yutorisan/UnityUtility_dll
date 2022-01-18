using System;
using Xunit;
using UnityUtility;
using System.Collections.Generic;
using UnityUtility.Linq;
using System.Diagnostics.CodeAnalysis;

namespace UnityUtilityUnitTest
{
    public class DictionaryExtensionsTest
    {
        private Dictionary<int, string> m_dic1 = new Dictionary<int, string>()
        {
            {1, "a" },
            {2, "b" },
            {3, "c" }
        };
        private Dictionary<int, string> m_dic2 = new Dictionary<int, string>()
        {
            {1, "A" },
            {3, "C" },
            {5, "E" }
        };
        private Dictionary<int, int> m_dic3 = new Dictionary<int, int>()
        {
            {1, 10 },
            {2, 20 },
            {4, 40 }
        };
        private Dictionary<int, string> m_empty = new Dictionary<int, string>();

        [Fact]
        public void DictionaryZipTest1()
        {
            var ziped1 = m_dic1.DictionaryZip(m_dic2, (s1, s2) => s1 + s2).GetEnumerator();
            var ziped2 = m_dic2.DictionaryZip(m_dic1, (s2, s1) => s2 + s1).GetEnumerator();
            var emptyZiped = m_empty.DictionaryZip(m_dic1, (emp, s1) => emp + s1).GetEnumerator();
            var zipedEmpty = m_dic1.DictionaryZip(m_empty, (s1, emp) => s1 + emp).GetEnumerator();

            //dic1+dic2
            ziped1.MoveNext().IsTrue();
            ziped1.Current.Is(new KeyValuePair<int, string>(1, "aA"));
            ziped1.MoveNext().IsTrue();
            ziped1.Current.Is(new KeyValuePair<int, string>(3, "cC"));
            ziped1.MoveNext().IsFalse();

            //dic2+dic1
            ziped2.MoveNext().IsTrue();
            ziped2.Current.Is(new KeyValuePair<int, string>(1, "Aa"));
            ziped2.MoveNext().IsTrue();
            ziped2.Current.Is(new KeyValuePair<int, string>(3, "Cc"));
            ziped2.MoveNext().IsFalse();

            //empty+dic1
            emptyZiped.MoveNext().IsFalse();

            //zip1+empty
            zipedEmpty.MoveNext().IsFalse();
        }

        [Fact]
        public void DictionatyCombineTest()
        {
            var combine1 = m_dic1.DictionaryCombine(m_dic2, (s1, s2) => s1 + s2).GetEnumerator();
            var combine2 = m_dic2.DictionaryCombine(m_dic1, (s2, s1) => s2 + s1).GetEnumerator();
            //var combine3 = m_dic1.DictionaryCombine(m_dic3, (s, n) => s + n * 2, "存在しない", 0).GetEnumerator();
            var emptyCombine = m_empty.DictionaryCombine(m_dic1, (emp, s1) => emp + s1).GetEnumerator();
            var combineEmpty = m_dic1.DictionaryCombine(m_empty, (s1, emp) => s1 + emp).GetEnumerator();

            combine1.MoveNext().IsTrue();
            combine1.Current.Is(new KeyValuePair<int, string>(1, "aA"));
            combine1.MoveNext().IsTrue();
            combine1.Current.Is(new KeyValuePair<int, string>(2, "b"));
            combine1.MoveNext().IsTrue();
            combine1.Current.Is(new KeyValuePair<int, string>(3, "cC"));
            combine1.MoveNext().IsTrue();
            combine1.Current.Is(new KeyValuePair<int, string>(5, "E"));
            combine1.MoveNext().IsFalse();

            combine2.MoveNext().IsTrue();
            combine2.Current.Is(new KeyValuePair<int, string>(1, "Aa"));
            combine2.MoveNext().IsTrue();
            combine2.Current.Is(new KeyValuePair<int, string>(3, "Cc"));
            combine2.MoveNext().IsTrue();
            combine2.Current.Is(new KeyValuePair<int, string>(5, "E"));
            combine2.MoveNext().IsTrue();
            combine2.Current.Is(new KeyValuePair<int, string>(2, "b"));
            combine2.MoveNext().IsFalse();

            //combine3.MoveNext().IsTrue();
            //combine3.Current.Is(new KeyValuePair<int, string>(1, "a20"));
            //combine3.MoveNext().IsTrue();
            //combine3.Current.Is(new KeyValuePair<int, string>(2, "b40"));
            //combine3.MoveNext().IsTrue();
            //combine3.Current.Is(new KeyValuePair<int, string>(3, "c0"));
            //combine3.MoveNext().IsTrue();
            //combine3.Current.Is(new KeyValuePair<int, string>(4, "存在しない80"));
            //combine3.MoveNext().IsFalse();

            emptyCombine.MoveNext().IsTrue();
            emptyCombine.Current.Is(new KeyValuePair<int, string>(1, "a"));
            emptyCombine.MoveNext().IsTrue();
            emptyCombine.Current.Is(new KeyValuePair<int, string>(2, "b"));
            emptyCombine.MoveNext().IsTrue();
            emptyCombine.Current.Is(new KeyValuePair<int, string>(3, "c"));
            emptyCombine.MoveNext().IsFalse();

            combineEmpty.MoveNext().IsTrue();
            combineEmpty.Current.Is(new KeyValuePair<int, string>(1, "a"));
            combineEmpty.MoveNext().IsTrue();
            combineEmpty.Current.Is(new KeyValuePair<int, string>(2, "b"));
            combineEmpty.MoveNext().IsTrue();
            combineEmpty.Current.Is(new KeyValuePair<int, string>(3, "c"));
            combineEmpty.MoveNext().IsFalse();
        }

        [Fact]
        public void MaxMinBy()
        {
            List<TwoPairInt> twoPairInts = new List<TwoPairInt>()
            {
                new TwoPairInt(1, 9),
                new TwoPairInt(2, 8),
                new TwoPairInt(5, 5),
                new TwoPairInt(4, 6),
                new TwoPairInt(3, 7),
                new TwoPairInt(1, 8),
            };

            twoPairInts.MaxBy(pair => pair.Value1).Is(new TwoPairInt(5, 5));
            twoPairInts.MaxBy(pair => pair.Value2).Is(new TwoPairInt(1, 9));
            twoPairInts.MinBy(pair => pair.Value1).Is(new TwoPairInt(1, 9), new TwoPairInt(1, 8));
            twoPairInts.MinBy(pair => pair.Value2).Is(new TwoPairInt(5, 5));

            //twoPairInts.MaxByWithIndex(pair => pair.Value1).Index.Is(2);
            //twoPairInts.MinByWithIndex(pair => pair.Value1).Index.Is(0);
            //twoPairInts.MinByWithIndexLast(pair => pair.Value1).Index.Is(5);
        }

        readonly struct TwoPairInt : IEquatable<TwoPairInt>
        {
            public TwoPairInt(int int1, int int2)
            {
                Value1 = int1;
                Value2 = int2;
            }

            public int Value1 { get; }
            public int Value2 { get; }

            public bool Equals([AllowNull] TwoPairInt other)
            {
                return this.Value1 == other.Value1 && this.Value2 == other.Value2;
            }
        }
    }
}
