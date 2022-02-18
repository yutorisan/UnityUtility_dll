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
        /// <param name="size">このサイズに修正する。指定しなければ1x1</param>
        public static void NormalizeSize(this SpriteRenderer renderer, Vector2? size = null)
        {
            size ??= Vector2.one;
            float sizeX = renderer.bounds.size.x;
            float sizeY = renderer.bounds.size.y;
            float scaleX = size.Value.x / sizeX;
            float scaleY = size.Value.y / sizeY;
            renderer.gameObject.transform.localScale = new Vector2(scaleX, scaleY);
        }
    }
}
