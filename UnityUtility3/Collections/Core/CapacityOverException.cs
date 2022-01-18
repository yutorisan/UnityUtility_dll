using System;
using System.Runtime.Serialization;

namespace UnityUtility.Collections.Core
{
    /// <summary>
    /// キャパシティが限定されているコレクションで、キャパシティを超えて要素を追加したときにスローされる
    /// </summary>
    internal class CapacityOverException : Exception
    {
        internal CapacityOverException() : base("キャパシティを超えて要素を追加しようとしました")
        {
        }
        internal CapacityOverException(string message) : base(message)
        {
        }
        internal CapacityOverException(string message, Exception innerException) : base(message, innerException)
        {
        }
        protected CapacityOverException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

}
