using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityUtility.Extensions
{
    public static class UnityExtensions
    {
        /// <summary>
        /// このコンポーネントをアタッチしているGameObjectの子オブジェクトのTransformをすべて取得します。
        /// 次の式に等しい：this.transform.Cast<Transform>()
        /// </summary>
        /// <param name="mono"></param>
        /// <returns></returns>
        public static IEnumerable<Transform> GetChildTransforms(this MonoBehaviour mono) => mono.transform.Cast<Transform>();
    }
}
