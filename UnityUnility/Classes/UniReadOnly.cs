using System;
namespace UnityUtility.Classes
{
    /// <summary>
    /// 一度しか初期化できない値です。
    /// readonlyキーワードの代わりに使用します。
    /// </summary>
    /// <typeparam name="T">ラップする型</typeparam>
    public class UniReadOnly<T>
    {
        private bool m_isInitialized = false;

        /// <summary>
        /// 値を初期化します。このメソッドは一度しか呼ぶことができません。
        /// </summary>
        /// <param name="value">初期化する値</param>
        /// <exception cref="AlreadyInitializedException">複数回初期化しようとしたときにスローされます。</exception>
        public void Initialize(T value)
        {
            if (m_isInitialized) throw new AlreadyInitializedException();
            Value = value;
            m_isInitialized = true;
        }

        /// <summary>
        /// 値を取得します。
        /// </summary>
        public T Value { get; private set; }

        private class AlreadyInitializedException : Exception
        {
            public AlreadyInitializedException() : base("すでに初期化されています。")
            {
            }
        }
    }
}
