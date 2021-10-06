using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtension
{
    public static Transform Find(this Transform childTransformList, Predicate<Transform> predicate)
    {
        foreach (Transform childTransform in childTransformList)
        {
            if (predicate.Invoke(childTransform))
                return childTransform;
        }
        return null;
    }
    public static void DestroyAllChildren(this Transform parent)
    {
        foreach (Transform each in parent)
            UnityEngine.Object.Destroy(each.gameObject);
    }
}
