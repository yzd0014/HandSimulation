using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class palmRotation : MonoBehaviour
{

    public Transform child;
    public Transform parent;

    private void FixedUpdate()
    {

        //get world transform of palm

        transform.rotation = Quaternion.Inverse(worldToLocal.getRotation(parent, child));


    }



}
