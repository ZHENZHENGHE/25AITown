using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformUtilty
{
    public static Transform find(Transform obj,string name)
    {
        if (obj.Find(name))
        {
            return obj.Find(name);
        }
        for (int i = 0; i < obj.childCount; i++)
        {
            Transform child = obj.GetChild(i);
            if (find(child, name))
            {
                return find(child, name);
            }
        }
        return null;
    }
}
