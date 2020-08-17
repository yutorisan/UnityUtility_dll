using System;
using UnityEngine;

namespace UnityUtility
{
    public static class MathExtensions
    {
        public static double ToRadian(this double degree) => degree * Mathf.PI / 180;
        public static double ToDegree(this double radian) => radian * 180 / Mathf.PI;

    }
}
