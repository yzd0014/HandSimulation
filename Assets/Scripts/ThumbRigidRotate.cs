using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThumbRigidRotate : MonoBehaviour
{

    public Rigidbody RB;
    public Transform palmBase;
    public Transform middleBase;

    public Transform Parent;
    public Transform TransParent;

    public bool updatePosition;
    public bool updateRotation;
    public bool force;

    // Start is called before the first frame update


    protected const float MaxVelocityChange = 1f;
    protected const float VelocityMagic = 100000f;
    protected const float AngularVelocityMagic = 5000f;
    protected const float MaxAngularVelocityChange = 100f;


    public float MaxForceChange = 1f;
    public float ForceMagic = 1000f;
    public float MaxTorqueChange = 1f;
    public float TorqueMagic = 1000f;

    public float ForceDamp = 10f;
    public float TorqueDamp = 1f;
    Vector3 prevVel;
    Vector3 prevTrq;

    // Update is called once per frame
    void FixedUpdate()
    {

        UpdateVelocity();

    }

    Quaternion getTransDirection()
    {
        Vector3 direction = (middleBase.position - palmBase.position).normalized;
        return Quaternion.LookRotation(direction, palmBase.up);


    }

    void UpdateVelocity()
    {

        Vector3 velocityTarget, angularTarget;
        bool success = GetUpdatedAttachedVelocities(out velocityTarget, out angularTarget);
        if (success)
        {

            if (updatePosition)
            {
                if (force)
                    RB.AddForce(((palmBase.position - RB.position) * ForceMagic) - (ForceDamp * RB.velocity));
                else
                    RB.velocity = Vector3.MoveTowards(RB.velocity, velocityTarget, MaxVelocityChange);
            }




            if (updateRotation)
            {



                //the angluar velocity in the damp needs to belocal, not global
                //we must subtract the parent's angular
                Rigidbody RBP;
                Vector3 AV;

                
                if(Parent != null)
                {
                    RBP = Parent.GetComponent<Rigidbody>();
                    AV = RB.angularVelocity - RBP.angularVelocity;
                }
                else
                {
                    AV = RB.angularVelocity;

                }
                

                if (force)
                    RB.AddTorque((getTorsionalSpringTorque(RB.transform, palmBase, TorqueMagic)) - (TorqueDamp* AV));
                else
                    RB.angularVelocity = Vector3.MoveTowards(RB.angularVelocity, angularTarget, MaxAngularVelocityChange);



            }
            //RB.angularVelocity = (Quaternion.RotateTowards(Quaternion.Euler(RB.angularVelocity), rotTarget, MaxAngularVelocityChange)).eulerAngles;
        }

        //        RB.rotation = Quaternion.Slerp(Quaternion.identity, Quaternion.Euler(45, 0, 0), Mathf.Sin(Time.time));

    }

    Vector3 getTorsionalSpringTorque(Transform current, Transform target, float torPow)
    {

        //T = q * r * a1
        //T = torque
        //q = spring constant
        //r = angle difference
        //a1 = axis of rotation

        //with the transform up and the normal we're targeting, we can use the cross product to find our rotation axis

        //

        Vector3 T = Vector3.zero;




        if (Parent != null)
        {

            Quaternion tRot = getTransDirection();
            Matrix4x4 trackWorldRotMat = Matrix4x4.Rotate(tRot);
            Matrix4x4 TPWTLMat = TransParent.worldToLocalMatrix;
            Matrix4x4 localMat = trackWorldRotMat * TPWTLMat;

            Matrix4x4 PLTWMat = Parent.localToWorldMatrix;
            Matrix4x4 finalMat = localMat * PLTWMat;



            /*
            if (drawLines)
            {
                Debug.DrawLine(RB.position, RB.position + (targetWorld * 0.25f), Color.green);
                Debug.DrawLine(RB.position, RB.position + (RB.transform.up * 0.25f), Color.red);
                Debug.DrawLine(trans.position, trans.position + (trans.up * 0.25f), Color.blue);
            }
            */


            Quaternion targetItemRotation = finalMat.rotation;
            Quaternion rotationDelta = targetItemRotation * Quaternion.Inverse(transform.rotation);


            float angle;
            Vector3 axis;
            rotationDelta.ToAngleAxis(out angle, out axis);

            if (angle > 180)
                angle -= 360;

            if (angle != 0 && float.IsNaN(axis.x) == false && float.IsInfinity(axis.x) == false)
            {
                T += angle * axis * torPow;
            }

            else
                T = Vector3.zero;


        }
        else
        {

            Quaternion targetItemRotation = getTransDirection();
            Quaternion rotationDelta = targetItemRotation * Quaternion.Inverse(transform.rotation);


            float angle;
            Vector3 axis;
            rotationDelta.ToAngleAxis(out angle, out axis);

            if (angle > 180)
                angle -= 360;

            if (angle != 0 && float.IsNaN(axis.x) == false && float.IsInfinity(axis.x) == false)
            {
                T += angle * axis * torPow;
            }

            else
                T = Vector3.zero;



        }




        return T;


    }

    protected bool GetUpdatedAttachedVelocities(out Vector3 velocityTarget, out Vector3 angularTarget)
    {
        bool realNumbers = false;

        //set the magic values... not sure of their purpose?
        float velocityMagic = VelocityMagic;
        float angularVelocityMagic = AngularVelocityMagic;

        //get direction toward the original wrist 
        Vector3 targetItemPosition = palmBase.position;
        Vector3 positionDelta = (targetItemPosition - RB.position);


        velocityTarget = (positionDelta * velocityMagic * Time.deltaTime);

        if (float.IsNaN(velocityTarget.x) == false && float.IsInfinity(velocityTarget.x) == false)
        {
            realNumbers = true;
        }
        else
            velocityTarget = Vector3.zero;


        Quaternion targetItemRotation = getTransDirection();
        Quaternion rotationDelta = targetItemRotation * Quaternion.Inverse(transform.rotation);


        float angle;
        Vector3 axis;
        rotationDelta.ToAngleAxis(out angle, out axis);

        if (angle > 180)
            angle -= 360;

        if (angle != 0 && float.IsNaN(axis.x) == false && float.IsInfinity(axis.x) == false)
        {
            angularTarget = angle * axis * angularVelocityMagic * Time.deltaTime;
            realNumbers &= true;
        }
        else
            angularTarget = Vector3.zero;

        return realNumbers;
    }
}

