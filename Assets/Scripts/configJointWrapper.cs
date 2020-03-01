using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class configJointWrapper : MonoBehaviour
{


    //holds the info of the rotations we want, as well as a spring constant

    public float springConstant;
    public ConfigurableJoint CJ;

    private void Start()
    {

        setDrive();

    }

    public void setTargetRotation(Quaternion Rot){

        CJ.targetRotation = Rot;

    }

    void setDrive()
    {

        JointDrive A = new JointDrive
        {
            positionSpring = springConstant,
            positionDamper = 0,
            maximumForce = CJ.angularXDrive.maximumForce
        };
        CJ.angularXDrive = A;
        CJ.angularYZDrive = A;

    }

}
