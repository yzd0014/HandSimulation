using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class torqueTest : MonoBehaviour
{

    public Transform RotFollow;
    public Rigidbody RB;
    public HingeJoint HJ;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        JointSpring JS = HJ.spring;


        float xAngle = Quaternion.Inverse(RotFollow.localRotation).eulerAngles.x;
        JS.targetPosition = xAngle < 180 ? xAngle : xAngle - 360; 
        HJ.spring = JS;
    }
}
