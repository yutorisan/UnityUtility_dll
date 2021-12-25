using System;
using UnityEngine;
namespace UnityUtility
{
    public static class GeneralExtensions
    {
        /// <summary>
        /// このオブジェクトをログ出力します
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        public static T Log<T>(this T obj, string label = null)
        {
            string log = label is null ? obj.ToString() : $"[{label}] {obj.ToString()}";
            UnityEngine.Debug.Log(log);
            return obj;
        }
    }
}

