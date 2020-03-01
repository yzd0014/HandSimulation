using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class worldToLocal
{

    public static Quaternion getRotation(Transform parent, Transform child)
    {

        Matrix4x4 m1 = child.worldToLocalMatrix;
        Matrix4x4 m2 = parent.localToWorldMatrix;
        Quaternion A = (m1 * m2).rotation;
        return A;

    }




}
