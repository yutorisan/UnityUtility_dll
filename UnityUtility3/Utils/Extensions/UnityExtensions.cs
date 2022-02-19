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

        /// <summary>
        /// Spriteの大きさを指定のサイズに修正する
        /// </summary>
        /// <param name="renderer"></param>
        /// <param name="targetSize">このサイズに修正する。指定しなければ1x1</param>
        public static void NormalizeSize(this SpriteRenderer renderer, Vector2? targetSize = null)
        {
            Vector2 size = targetSize ?? Vector2.one;
            Vector2 firstScale = renderer.transform.localScale;
            float boundX = renderer.bounds.size.x / firstScale.x;
            float boundY = renderer.bounds.size.y / firstScale.y;
            float scaleX = size.x / boundX;
            float scaleY = size.y / boundY;
            renderer.gameObject.transform.localScale = new Vector2(scaleX, scaleY);
        }
    }
}
