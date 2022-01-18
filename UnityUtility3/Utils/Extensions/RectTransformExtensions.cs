using System;
using UnityEngine;

namespace UnityUtility
{
    public static class RectTransformExtensions
    {
        public static Bounds GetWorldBounds(this RectTransform rect)
        {
            Vector3[] worldCorners = new Vector3[4];
            rect.GetWorldCorners(worldCorners);
            return new Bounds(
                worldCorners.GetCenter(),
                new Vector3(worldCorners[2].x - worldCorners[0].x,
                            worldCorners[2].y - worldCorners[0].y,
                            worldCorners[2].z - worldCorners[0].z));
        }
    }
}

