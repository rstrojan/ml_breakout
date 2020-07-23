using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
    // find child objects with specific tag
    public static T FindComponentChildWithTag<T>(this GameObject parent, string tag)where T:Component{
        Transform t = parent.transform;
        foreach(Transform tr in t){
            if(tr.tag == tag){
                return tr.GetComponent<T>();
            }
        }
        return null;
    }
}
