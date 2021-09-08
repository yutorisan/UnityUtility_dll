using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityUtility.Linq
{
    public static partial class EnumerableFactory
    {
        /// <summary>
        /// ランダムな整数値を含んだEnumerableシーケンスを生成します。
        /// </summary>
        /// <param name="min">生成する値の最小値（この値を含む）</param>
        /// <param name="max">生成する値の最大値（この値を含まない）</param>
        /// <param name="count">生成する値の数</param>
        /// <param name="seed"><see cref="UnityEngine.Random.InitState(int)"/>に引き渡すシード値</param>
        /// <returns></returns>
        public static IEnumerable<int> Random(int min, int max, int count, int seed)
        {
            if (max < min) throw new ArgumentOutOfRangeException($"{nameof(min)}と{nameof(max)}の指定の仕方が不正です");

            UnityEngine.Random.InitState(seed);
            for (int i = 0; i < count; i++)
            {
                yield return UnityEngine.Random.Range(min, max);
            }
        }
        /// <summary>
        /// ランダムな整数値を含んだEnumerableシーケンスを生成します。
        /// seedには現在のミリ秒が指定されます。
        /// </summary>
        /// <param name="min">生成する値の最小値（この値を含む）</param>
        /// <param name="max">生成する値の最大値（この値を含まない）</param>
        /// <param name="count">生成する値の数</param>
        /// <returns></returns>
        public static IEnumerable<int> Random(int min, int max, int count) =>
            Random(min, max, count, DateTime.Now.Millisecond);

        /// <summary>
        /// ランダムな実数値を含んだEnumerableシーケンスを生成します。
        /// </summary>
        /// <param name="min">生成する値の最小値（この値を含む）</param>
        /// <param name="max">生成する値の最大値（この値を含む）</param>
        /// <param name="count">生成する値の数</param>
        /// <param name="seed"><see cref="UnityEngine.Random.InitState(int)"/>に引き渡すシード値</param>
        /// <returns></returns>
        public static IEnumerable<float> Random(float min, float max, int count, int seed)
        {
            if (max < min) throw new ArgumentOutOfRangeException($"{nameof(min)}と{nameof(max)}の指定の仕方が不正です");

            UnityEngine.Random.InitState(seed);
            for (int i = 0; i < count; i++)
            {
                yield return UnityEngine.Random.Range(min, max);
            }
        }
        /// <summary>
        /// ランダムな実数値を含んだEnumerableシーケンスを生成します。
        /// seedには現在のミリ秒が指定されます。
        /// </summary>
        /// <param name="min">生成する値の最小値（この値を含む）</param>
        /// <param name="max">生成する値の最大値（この値を含まない）</param>
        /// <param name="count">生成する値の数</param>
        /// <returns></returns>
        public static IEnumerable<float> Random(float min, float max, int count) =>
            Random(min, max, count, DateTime.Now.Millisecond);

    }
}
