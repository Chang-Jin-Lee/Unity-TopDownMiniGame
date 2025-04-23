using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentHelper : MonoBehaviour
{
    public static T FindInterface<T>(GameObject target) where T : class
    {
        var components = target.GetComponents<MonoBehaviour>();

        foreach (var comp in components)
        {
            if (comp is T match)
            {
                return match;
            }
        }

        return null;
    }
}
