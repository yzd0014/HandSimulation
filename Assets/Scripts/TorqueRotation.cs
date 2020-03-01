using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorqueRotation : MonoBehaviour
{

    public Rigidbody RB;
    public HingeJoint HJ;



    // Update is called once per frame
    void FixedUpdate()
    {

        //RB.AddRelativeTorque(transform.right * getTorqueFromRotation(90), ForceMode.Force);
        //RB.AddRelativeTorque(transform.right * (HJ.spring.spring/Time.fixedDeltaTime));
        //RB.AddRelativeTorque(transform.right * (HJ.spring.spring));
        RB.AddRelativeTorque(transform.right * 1000);
    }

    float getTorqueFromRotation(float angle)
    {
        //using the basic torque thing (T = -k*theta), get T 
        //...that didn't work. 
        //maybe timestep needs to be involved

        float k = HJ.spring.spring;
        return (angle * k);

    }
}
