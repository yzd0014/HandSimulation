using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thumbKnuckleRotation : MonoBehaviour
{

    public Transform baseRef;
    public Transform rotRef;
    public Transform child;
    public Transform parent;


    //take direction from palm base to thumb piece
    //rotate thumb piece to match with palm base up as up
    //location is location

    private void FixedUpdate()
    {

        Vector3 direction = (rotRef.position - baseRef.position).normalized;
        child.rotation = Quaternion.LookRotation(direction, baseRef.up);
        child.position = baseRef.position;


        //get world transform of palm
        transform.rotation = Quaternion.Inverse(worldToLocal.getRotation(parent, child)) * Quaternion.Euler(90, 0, 0);


    }
}
