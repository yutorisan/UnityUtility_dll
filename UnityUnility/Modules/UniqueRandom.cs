using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityUtility.Classes
{
    /// <summary>
    /// 特定の範囲内で重複しないランダムな数値を出力するモジュール
    /// </summary>
    public class UniqueRandom
    {
        private readonly int m_min, m_max;
        private readonly List<int> m_alreadys = new List<int>();

        /// <summary>
        /// 出力範囲を指定して新規インスタンスを作成します
        /// </summary>
        /// <param name="min">出力する最小値</param>
        /// <param name="max">出力する最大値（この値を含まない）</param>
        public UniqueRandom(int min, int max, bool isAutoReset = false)
        {
            this.m_min = min;
            this.m_max = max;
            this.IsAutoReset = isAutoReset;
        }

        /// <summary>
        /// 重複しない値をランダムで取得します。
        /// </summary>
        /// <returns></returns>
        public int Pick()
        {
            if(m_alreadys.Count == m_max - m_min)
            {
                if (IsAutoReset) Reset();
                else throw new EveryValuePickupedException();
            }

            int val;
            do
            {
                val = Random.Range(m_min, m_max);
            } while (m_alreadys.Any(n => n == val));

            m_alreadys.Add(val);
            return val;
        }

        /// <summary>
        /// 今までに出現した値の記憶を破棄します
        /// </summary>
        public void Reset() => m_alreadys.Clear();

        /// <summary>
        /// すべての値が出現したらリセットするか
        /// </summary>
        public bool IsAutoReset { get; }

        private class EveryValuePickupedException : System.Exception { }
    }
}
