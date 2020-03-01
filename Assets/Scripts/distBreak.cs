using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class distBreak : MonoBehaviour
{

    Transform connected;
    ConfigurableJoint CJ;
    public float BreakDist;
    // Start is called before the first frame update
    void Start()
    {
        CJ = GetComponent<ConfigurableJoint>();
        connected = CJ.connectedBody.transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector3.Distance(connected.position, transform.position) > BreakDist)
        {
            CJ.breakForce = 0;
            this.enabled = false;
        }

    }
}
