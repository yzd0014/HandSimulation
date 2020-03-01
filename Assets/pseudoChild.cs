using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pseudoChild : MonoBehaviour
{

    public bool worldParent;
    public bool worldChild;

    public Transform parent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 newPos;
        Quaternion newRot;

        if (!worldParent)
        {
            newPos = parent.localPosition;
            newRot = parent.localRotation;

        }
        else
        {

            newPos = parent.position;
            newRot = parent.rotation;
        }


        if (!worldChild)
        {
            transform.localPosition = newPos;
            transform.localRotation = newRot;

        }

        else{

            transform.position = newPos;
            transform.rotation = newRot;

        }
        
        
        
    }
}
