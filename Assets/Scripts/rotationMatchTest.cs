using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotationMatchTest : MonoBehaviour
{
    public Transform track;
    public Transform Parent;
    public Transform tParent;
    Rigidbody RB;



    // Start is called before the first frame update
    void Start()
    {
        RB = GetComponent<Rigidbody>();    
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //get track local rotation
        //transform into world rotation relative to parent
        //transfer to rigid body

        Quaternion localTrack = track.rotation;
        Matrix4x4 trackLocalRotMat = Matrix4x4.Rotate(localTrack);
        Matrix4x4 tpwtlmat = tParent.worldToLocalMatrix;
        Matrix4x4 localmat = tpwtlmat * trackLocalRotMat;

        Matrix4x4 parentLTWMat = Parent.localToWorldMatrix;
        Matrix4x4 finalMat = localmat * parentLTWMat;



        Quaternion finalRot = finalMat.rotation;

        /*

        Quaternion localTrack = track.localRotation;
        Matrix4x4 trackLocalRotMat = Matrix4x4.Rotate(localTrack);
        Matrix4x4 parentLTWMat = Parent.localToWorldMatrix;
        Matrix4x4 finalMat = trackLocalRotMat * parentLTWMat;
        Quaternion finalRot = finalMat.rotation;
        */


        RB.rotation = finalRot;
        //RB.MoveRotation(finalRot);

        
    }
}
