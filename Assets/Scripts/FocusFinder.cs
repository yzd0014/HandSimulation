using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FocusFinder : MonoBehaviour
{

    public FocusData focusData;
    public Transform sword;

    //keep track of head position and rotation
    //keep track of gameobject name from raycast
    private void Start()
    {
        focusData = new FocusData();
        

    }


    void Update()
    {
        focusData.position = transform.position;
        focusData.rotation = transform.rotation;
        focusData.SwordPosition = sword.position;
        focusData.SwordRotation = sword.rotation;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
        {
            focusData.focusName = hit.transform.name;

        }
        else
        {
            focusData.focusName = "N/A";

        }
        
    }
}

[Serializable]
public class FocusData
{

    public Vector3 SwordPosition;
    public Quaternion SwordRotation;
    public Vector3 position;
    public Quaternion rotation;
    public string focusName;

    public FocusData()
    {
        SwordPosition = Vector3.zero;
        SwordRotation = Quaternion.identity;
        position = Vector3.zero;
        rotation = Quaternion.identity;
        focusName = "N/A";
    }


}
