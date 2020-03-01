using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class centerOfMass : MonoBehaviour
{

    public Transform COM;


    // Start is called before the first frame update
    void Start()
    {
        Rigidbody RB = transform.parent.GetComponent<Rigidbody>();
        RB.centerOfMass = COM.localPosition;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
