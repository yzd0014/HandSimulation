using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wristPosition : MonoBehaviour
{

    public Rigidbody RB;
    public Transform Wrist;


    private void FixedUpdate()
    {

        RB.MovePosition(Wrist.position);
        RB.MoveRotation(Wrist.rotation);

    }

}
