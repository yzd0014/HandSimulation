using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rigidRotate : MonoBehaviour
{

    //this object's rigid body
    public Rigidbody PhysicsJointRigidBody;
    //leap joint we're matching
    public Transform leapJoint;

    //this object's inboard joint
    public Transform PhysicsJointParent;
    //leap joint's inboard joint
    public Transform LeapJointParent;

    //are we matching the leap position?
    public bool updatePosition;
    //are we matching the leap rotation?
    public bool updateRotation;
    //are we usign force to reach our targets or are we settign velocities?
    public bool force;

    //wether or not we're doing kinematic tracking (set before play, not during)
    public bool KinematicTracking;
    public bool drawLines;

    // Start is called before the first frame update
    void Start()
    {

        
        if (KinematicTracking){

            PhysicsJointRigidBody.isKinematic = true;
        }
        else
        {
            PhysicsJointRigidBody.isKinematic = false;
        }
        

    }


    //velocity ttracking variables
    public float MaxVelocityChange = 1f;
    public float VelocityMagic = 100000f;
    public float MaxAngularVelocityChange = 100f;
    public float AngularVelocityMagic = 5000f;
    

    //force tracking variables
    public float ForceStiffness = 1000f;
    public float ForceDamp = 10f;
    public float TorqueStiffness = 1000f;
    public float TorqueDamp = 1f;
    Vector3 prevVel;
    Vector3 prevTrq;


    private void Update()
    {
        
        if (KinematicTracking)
        {

            transform.position = leapJoint.position;
            transform.rotation = leapJoint.rotation;

        }
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       

        if (KinematicTracking)
        {
            /*
            PhysicsJointRigidBody.MovePosition(leapJoint.position);
            PhysicsJointRigidBody.MoveRotation(leapJoint.rotation);
            */
            //transform.position = leapJoint.position;
            //transform.rotation = leapJoint.rotation;
        }
        else
        {
            UpdateVelocity();
        }
        if (drawLines)
        {

            Debug.DrawLine(transform.position, transform.position + PhysicsJointParent.TransformDirection(LeapJointParent.InverseTransformDirection(leapJoint.forward)));
            Debug.DrawLine(transform.position, transform.position + transform.forward);


        }

    }

    void UpdateVelocity()
    {

        Vector3 velocityTarget, angularTarget;
        bool success = GetUpdatedAttachedVelocities(out velocityTarget, out angularTarget);
        if (success)
        {
            //if we successfully got updated velocity/force targets, we update the, accordingly. 

            if (updatePosition)
            {
                //if we're using force, use the force variables and add to the rigid body force.
                if (force)
                    PhysicsJointRigidBody.AddForce(((leapJoint.position - PhysicsJointRigidBody.position) * ForceStiffness) - (ForceDamp * PhysicsJointRigidBody.velocity));
                //else, set the velocity
                else
                    PhysicsJointRigidBody.velocity = Vector3.MoveTowards(PhysicsJointRigidBody.velocity, velocityTarget, MaxVelocityChange);
            }




            if (updateRotation)
            {

                Rigidbody ParentJointRigidBody;
                Vector3 AngularVelocity;

                //if we have an inboard joint, find the angular velocity relavtive to the inboard joint
                if (PhysicsJointParent != null)
                {
                    ParentJointRigidBody = PhysicsJointParent.GetComponent<Rigidbody>();
                    AngularVelocity = PhysicsJointRigidBody.angularVelocity - ParentJointRigidBody.angularVelocity;
                }
                else
                {
                    AngularVelocity = PhysicsJointRigidBody.angularVelocity;

                }

                //if we're using force, use the torque variables and add to the rigid body torque.
                if (force)
                    PhysicsJointRigidBody.AddTorque((getTorsionalSpringTorque())-(TorqueDamp * AngularVelocity));
                //else, set the velocity
                else
                    PhysicsJointRigidBody.angularVelocity = Vector3.MoveTowards(PhysicsJointRigidBody.angularVelocity, angularTarget, MaxAngularVelocityChange);



            }
                
        }


    }


    Vector3 getTorsionalSpringTorque()
    {


        Vector3 TorqueOut = Vector3.zero;
        Quaternion targetItemRotation;

        if (PhysicsJointParent != null)
        {
            //get local rotation or joint and parent


            //Rlj = Ri^T * Ro
            Quaternion PhysicsJointRotation = transform.rotation;
            Matrix4x4 PhysicsRotationMatrix = Matrix4x4.Rotate(PhysicsJointRotation);
            Matrix4x4 PhysicsParentWTL = PhysicsJointParent.worldToLocalMatrix;
            Matrix4x4 CurrentLocalRotation = PhysicsParentWTL * PhysicsRotationMatrix;
            
            //dRlj = dRi^T * dRo
            Quaternion leapRotation = leapJoint.rotation;
            Matrix4x4 leapRotationMatrix = Matrix4x4.Rotate(leapRotation);
            Matrix4x4 leapParentWTL = LeapJointParent.worldToLocalMatrix;
            Matrix4x4 TargetLocalRotation = leapParentWTL * leapRotationMatrix;

            
            //eRl = dRlj * Rlj^T
            Matrix4x4 ErrorLocalRotation = TargetLocalRotation * Matrix4x4.Inverse(CurrentLocalRotation);
            Matrix4x4 PhysicsParentLTW = PhysicsJointParent.localToWorldMatrix;

            //eRw = Ri * eRl * Ri^T 
            Matrix4x4 ErrorWorldRotation = PhysicsParentLTW * ErrorLocalRotation * PhysicsParentWTL;


            targetItemRotation = ErrorWorldRotation.rotation;
            
            //Matrix4x4 PhysicsParentLTW = PhysicsJointParent.localToWorldMatrix;
            //Matrix4x4 TargetWorldRotation = PhysicsParentLTW * TargetLocalRotation;
            //targetItemRotation = TargetWorldRotation.rotation;

            

        }
        else
        {

            //eR = dR * R^T
            targetItemRotation = leapJoint.rotation * Quaternion.Inverse(transform.rotation);
            //targetItemRotation = leapJoint.rotation;


        }


        //Quaternion uErrorRotation = targetItemRotation * Quaternion.Inverse(transform.rotation);
        Quaternion uErrorRotation = targetItemRotation;

        float uErrorAngle;
        Vector3 uErrorAxis;
        uErrorRotation.ToAngleAxis(out uErrorAngle, out uErrorAxis);
        if (uErrorAngle > 180)
            uErrorAngle -= 360;

        if (uErrorAngle != 0 && float.IsNaN(uErrorAxis.x) == false && float.IsInfinity(uErrorAxis.x) == false)
        {
            TorqueOut += TorqueStiffness * uErrorAngle * uErrorAxis;
        }

        else
            TorqueOut = Vector3.zero;

        return TorqueOut;


    }

    protected bool GetUpdatedAttachedVelocities(out Vector3 velocityTarget, out Vector3 angularTarget)
    {
        bool realNumbers = false;


        float velocityMagic = !force?  VelocityMagic : ForceStiffness;
        float angularVelocityMagic = !force? AngularVelocityMagic : TorqueStiffness;

        Vector3 targetItemPosition = leapJoint.position;
        Vector3 positionDelta = (targetItemPosition - PhysicsJointRigidBody.position);


        velocityTarget = (positionDelta * velocityMagic * Time.deltaTime);

        if (float.IsNaN(velocityTarget.x) == false && float.IsInfinity(velocityTarget.x) == false)
        {
            realNumbers = true;
        }
        else
            velocityTarget = Vector3.zero;


        Quaternion targetItemRotation = leapJoint.rotation;
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


