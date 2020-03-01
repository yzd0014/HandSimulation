using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class configTorque : MonoBehaviour
{

    public Transform RotFollow;
    public ConfigurableJoint CJ;

    // Update is called once per frame
    void Update()
    {
        CJ.targetRotation = RotFollow.localRotation;
    }
}
