using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class middleManJointRotaion : MonoBehaviour
{

    public Transform leapJoint;
    public Transform parent;
    public Transform leapJointInboard;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(parent!=null)
        {
            Matrix4x4 leapJointRotation = Matrix4x4.Rotate(leapJoint.rotation);
            Matrix4x4 parentWTL = parent.worldToLocalMatrix;
            Matrix4x4 leapLocalRotation = parentWTL * leapJointRotation;
            transform.localRotation = leapLocalRotation.rotation;
        }
        else
        {

            transform.rotation = leapJoint.rotation;

        }
        

        /*
     Matrix4x4 leapJointRotation = Matrix4x4.Rotate(leapJoint.rotation);
     Matrix4x4 inboardWTL = leapJointInboard.worldToLocalMatrix;
     Matrix4x4 leapLocal = leapJointRotation * inboardWTL;
     transform.rotation = leapLocal.rotation;
     */

    }
}
