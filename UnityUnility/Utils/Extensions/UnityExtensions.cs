using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityUtility
{
    public static class UnityExtensions
    {
        /// <summary>
        /// same as <code>.transform.Cast&lt;Transform&gt;()</code>
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public static IEnumerable<Transform> Transforms(this Component component) => component.transform.Cast<Transform>();
    }
}
