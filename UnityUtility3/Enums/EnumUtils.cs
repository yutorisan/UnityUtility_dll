using System.Collections.Generic;
using System.Linq;

namespace UnityUtility.Enums
{
    public static class EnumUtils
    {
        /// <summary>
        /// 列挙型のすべてのメンバを取得します。
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static IEnumerable<TEnum> All<TEnum>() where TEnum : System.Enum =>
            System.Enum.GetValues(typeof(TEnum)).Cast<TEnum>();

        /// <summary>
        /// 列挙型のメンバ総数を取得します。
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static int Count<TEnum>() where TEnum : System.Enum =>
            System.Enum.GetNames(typeof(TEnum)).Length;

        /// <summary>
        /// 列挙体のメンバの中からランダムに1つ返します。
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static TEnum Random<TEnum>() where TEnum : System.Enum =>
            All<TEnum>().ElementAt(UnityEngine.Random.Range(0, Count<TEnum>()));
    }
}
