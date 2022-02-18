using System;
using System.Linq;
using Xunit;
using UnityUtility.Collections;
using UnityUtility.Modules;
using System.Collections.Generic;
using System.ComponentModel;
using UniRx;

namespace UnityUtilityUnitTest
{
    public class MapTest
    {
        [Fact]
        public void ConstructTest()
        {
            //plainなしコンストラクタ
            Map<int> map = new Map<int>(100, 50);
            map[0, 0].Is(0);
            map[99, 49].Is(0);

            //plainありコンストラクタ
            Map<string> plainMap = new Map<string>(50, 100, "plain");
            plainMap[37, 90].Is("plain");
            plainMap[20, 40] = "aaa";
            plainMap[20, 40].Is("aaa");

            //一次元配列コンストラクタ
            int[] int1dArray = Enumerable.Range(0, 100).ToArray();
            Map<int> array1Map = new Map<int>(10, 10, int1dArray);
            array1Map[2, 5].Is(52);
            Assert.Throws<ArgumentException>(() => new Map<int>(10, 9, int1dArray));

            //二次元配列コンストラクタ
            int[,] int2Array = new int[5, 3]
            {
                {1,2,3 },
                {4,5,6 },
                {7,8,9 },
                {10,11,12 },
                {13,14,15 }
            };
            Map<int> array2Map = new Map<int>(int2Array);
            array2Map[2, 3].Is(12);
            array2Map.ColumnCount.Is(3);

            //範囲外コンストラクタ
            Assert.Throws<ArgumentOutOfRangeException>(() => new Map<int>(0, 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new Map<int>(1, 0));

            //ReadOnlyプロパティチェック
            map.ColumnCount.Is(100);
            map.RowCount.Is(50);
            map.PlainValue.Is(default);
            map.HasPlain.IsTrue();
            plainMap.ColumnCount.Is(50);
            plainMap.RowCount.Is(100);
            plainMap.PlainValue.Is("plain");
            plainMap.HasPlain.IsTrue();

            map.ReWriteAll(_ => 1);
            map.HasPlain.IsFalse();
            plainMap.ReWriteAll(_ => default);
            plainMap.HasPlain.IsFalse();
        }

        public static IEnumerable<object[]> getIndexerTestData()
        {
            yield return new object[] { new Map<int>(10, 10) };
            yield return new object[] { new MapSlim<int>(10, 10) };
        }
        [Theory]
        [MemberData(nameof(getIndexerTestData))]
        public void IndexerTest(IMap<int> map)
        {
            //取得と書き換え
            map[4, 6] = 5;
            map[6, 4] = 6;
            map[4, 6].Is(5);
            map[6, 4].Is(6);
            //範囲外アクセス
            Assert.Throws<IndexOutOfRangeException>(() => map[10, 9]);
            Assert.Throws<IndexOutOfRangeException>(() => map[9, 10]);
            Assert.Throws<IndexOutOfRangeException>(() => map[-1, 4]);
            Assert.Throws<IndexOutOfRangeException>(() => map[4, -1]);
        }

        [Fact]
        public void ReWriteTest()
        {
            Map<string> stringMap = new Map<string>(1000, 1000);
            string testStr = "test";
            stringMap.ReWriteAll(str => testStr);
            stringMap[0, 0].Is(testStr);
            stringMap[500, 500].Is(testStr);
            stringMap[999, 999].Is(testStr);

            string rewritecolStr = "reWritedColumn";
            stringMap.ReWriteColumn(100, test => rewritecolStr);
            stringMap[100, 0].Is(rewritecolStr);
            stringMap[100, 400].Is(rewritecolStr);
            stringMap[100, 999].Is(rewritecolStr);
            stringMap[101, 40].Is(testStr);

            stringMap.ReWriteRow(500, str => str + "Row");
            stringMap[0, 500].Is("testRow");
            stringMap[100, 500].Is(rewritecolStr + "Row");

            Assert.Throws<ArgumentOutOfRangeException>(() => stringMap.ReWriteColumn(-1, _ => ""));
            Assert.Throws<ArgumentOutOfRangeException>(() => stringMap.ReWriteRow(1000, _ => ""));
        }

        [Fact]
        public void EnumerateTest()
        {
            Map<int> map = new Map<int>(1000, 2000, Enumerable.Range(0, 1000 * 2000).ToArray());
            map.Select(n => n * 2).ElementAt(100000).Is(200000);
            map.GetColumnEnumerable(2).ElementAt(2).Is(2002);
            map.GetRowEnumerable(1999).ElementAt(500).Is(1000 * 1999 + 500);
        }

        [Fact]
        public void SpanTest()
        {
            MapSlim<int> slimMap = new MapSlim<int>(1000, 2000, Enumerable.Range(0, 1000 * 2000).ToArray());
            foreach (ref var item in slimMap.GetRowSpan(100))
            {
                item = 0;
            }
            slimMap[100, 100].Is(0);
            slimMap[200, 100].Is(0);
        }

        [Fact]
        public void ExpandTest()
        {
            Map<int> source5x6 = new Map<int>(new int[,]
            {
                {1,2,3,4,5 },
                {6,7,8,9,10 },
                {11,12,13,14,15 },
                {16,17,18,19,20 },
                {21,22,23,24,25 },
                {26,27,28,29,30 }
            });
            var clone5x6 = source5x6.Clone();
            // 5x5 を左に2拡張
            clone5x6.ExpandColumn(UnityUtility.Enums.LeftRight.Left, 2);
            clone5x6.ColumnCount.Is(7);
            clone5x6.SequenceEqual(new int[]
            {
                00,00,01,02,03,04,05,
                00,00,06,07,08,09,10,
                00,00,11,12,13,14,15,
                00,00,16,17,18,19,20,
                00,00,21,22,23,24,25,
                00,00,26,27,28,29,30
            }).IsTrue();
            clone5x6[6, 2].Is(15);
            clone5x6[0, 2].Is(0);
            Assert.Throws<IndexOutOfRangeException>(() => clone5x6[7, 0]);

            clone5x6 = source5x6.Clone();
            // 5x5を左に2縮小
            clone5x6.ExpandColumn(UnityUtility.Enums.LeftRight.Left, -2);
            clone5x6.ColumnCount.Is(3);
            clone5x6.SequenceEqual(new int[]
            {
                03,04,05,
                08,09,10,
                13,14,15,
                18,19,20,
                23,24,25,
                28,29,30
            }).IsTrue();
            clone5x6[2, 2].Is(15);
            clone5x6[0, 2].Is(13);
            Assert.Throws<IndexOutOfRangeException>(() => clone5x6[3, 0]);

            clone5x6 = source5x6.Clone();
            //5x6を右に2拡張
            clone5x6.ExpandColumn(UnityUtility.Enums.LeftRight.Right, 2);
            clone5x6.ColumnCount.Is(7);
            clone5x6.SequenceEqual(new int[]
            {
                01,02,03,04,05,00,00,
                06,07,08,09,10,00,00,
                11,12,13,14,15,00,00,
                16,17,18,19,20,00,00,
                21,22,23,24,25,00,00,
                26,27,28,29,30,00,00,
            }).IsTrue();
            clone5x6[6, 3].Is(0);
            clone5x6[0, 3].Is(16);
            Assert.Throws<IndexOutOfRangeException>(() => clone5x6[7, 0]);

            clone5x6 = source5x6.Clone();
            //5x6を右に2縮小
            clone5x6.ExpandColumn(UnityUtility.Enums.LeftRight.Right, -2);
            clone5x6.ColumnCount.Is(3);
            clone5x6.SequenceEqual(new int[]
            {
                01,02,03,
                06,07,08,
                11,12,13,
                16,17,18,
                21,22,23,
                26,27,28,
            }).IsTrue();
            clone5x6[2, 1].Is(8);
            clone5x6[0, 3].Is(16);
            Assert.Throws<IndexOutOfRangeException>(() => clone5x6[3, 2]);

            clone5x6 = source5x6.Clone();
            //5x6を上に2拡張
            clone5x6.ExpandRow(UnityUtility.Enums.UpDown.Up, 2);
            clone5x6.RowCount.Is(8);
            clone5x6.SequenceEqual(new int[]
            {
                00,00,00,00,00,
                00,00,00,00,00,
                01,02,03,04,05,
                06,07,08,09,10,
                11,12,13,14,15,
                16,17,18,19,20,
                21,22,23,24,25,
                26,27,28,29,30
            }).IsTrue();
            clone5x6[2, 7].Is(28);
            clone5x6[1, 1].Is(0);
            Assert.Throws<IndexOutOfRangeException>(() => clone5x6[2, -1]);
            Assert.Throws<IndexOutOfRangeException>(() => clone5x6[2, 8]);

            clone5x6 = source5x6.Clone();
            //5x6を上に2縮小
            clone5x6.ExpandRow(UnityUtility.Enums.UpDown.Up, -2);
            clone5x6.RowCount.Is(4);
            clone5x6.SequenceEqual(new int[]
            {
                11,12,13,14,15,
                16,17,18,19,20,
                21,22,23,24,25,
                26,27,28,29,30
            }).IsTrue();
            clone5x6[4, 0].Is(15);
            clone5x6[0, 3].Is(26);
            Assert.Throws<IndexOutOfRangeException>(() => clone5x6[0, 4]);

            clone5x6 = source5x6.Clone();
            //5x6を下に2拡張
            clone5x6.ExpandRow(UnityUtility.Enums.UpDown.Down, 2);
            clone5x6.RowCount.Is(8);
            clone5x6.SequenceEqual(new int[]
            {
                01,02,03,04,05,
                06,07,08,09,10,
                11,12,13,14,15,
                16,17,18,19,20,
                21,22,23,24,25,
                26,27,28,29,30,
                00,00,00,00,00,
                00,00,00,00,00,
            }).IsTrue();

            clone5x6 = source5x6.Clone();
            //5x6を下に２縮小
            clone5x6.ExpandRow(UnityUtility.Enums.UpDown.Down, -2);
            clone5x6.RowCount.Is(4);
            clone5x6.SequenceEqual(new int[]
            {
                01,02,03,04,05,
                06,07,08,09,10,
                11,12,13,14,15,
                16,17,18,19,20,
            }).IsTrue();
            Assert.Throws<IndexOutOfRangeException>(() => clone5x6[3, 4]);

            //縮小なし
            clone5x6 = source5x6.Clone();
            clone5x6.ExpandColumn(UnityUtility.Enums.LeftRight.Left, 0);
            clone5x6.SequenceEqual(source5x6).IsTrue();
            clone5x6.ExpandColumn(UnityUtility.Enums.LeftRight.Right, 0);
            clone5x6.SequenceEqual(source5x6).IsTrue();
            clone5x6.ExpandRow(UnityUtility.Enums.UpDown.Up, 0);
            clone5x6.SequenceEqual(source5x6).IsTrue();
            clone5x6.ExpandRow(UnityUtility.Enums.UpDown.Down, 0);
            clone5x6.SequenceEqual(source5x6).IsTrue();

            //例外
            Assert.Throws<ArgumentOutOfRangeException>(() => source5x6.ExpandRow(UnityUtility.Enums.UpDown.Down, int.MaxValue));
            Assert.Throws<ArgumentOutOfRangeException>(() => source5x6.ExpandColumn(UnityUtility.Enums.LeftRight.Left, int.MaxValue));
            Assert.Throws<ArgumentOutOfRangeException>(() => source5x6.ExpandRow(UnityUtility.Enums.UpDown.Up, -6));
            Assert.Throws<ArgumentOutOfRangeException>(() => source5x6.ExpandRow(UnityUtility.Enums.UpDown.Down, -6));
            Assert.Throws<ArgumentOutOfRangeException>(() => source5x6.ExpandColumn(UnityUtility.Enums.LeftRight.Left, -5));
            Assert.Throws<ArgumentOutOfRangeException>(() => source5x6.ExpandColumn(UnityUtility.Enums.LeftRight.Right, -5));
        }
    }
}

