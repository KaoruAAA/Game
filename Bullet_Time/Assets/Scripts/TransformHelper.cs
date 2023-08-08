using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformHelper 
{
   public static Transform DeepFind(this Transform parent, string targetName)
    {
        foreach(Transform child in parent)
        {
            Debug.Log(child.name );
        }
        return null;
    }
}
