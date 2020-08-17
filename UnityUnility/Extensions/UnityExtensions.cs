using System;
using UnityEngine;

namespace UnityUtility
{
    public static class UnityExtensions
    {
        public static Transform GetChildTransforms(this GameObject mono) => mono.transform;
        public static Transform GetChildTransform(this GameObject mono, string name) => GetChildTransforms(mono).Find(name);
        public static GameObject GetChildGameObject(this GameObject mono, string name) => GetChildTransform(mono, name).gameObject;

    }
}
