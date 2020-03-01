using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HandFinder : MonoBehaviour
{

    public int ID;
    public GameObject hand;
    private ConfigurableJoint[] CF;
    private rigidRotate RR;

    public targetInfo TI;


    // Start is called before the first frame update
    void Start()
    {

        CF = hand.transform.GetComponentsInChildren<ConfigurableJoint>();
        RR = hand.transform.GetComponentInChildren<rigidRotate>();
        TI = new targetInfo();
        TI.ID = ID;
       
        
    }

    // Update is called once per frame
    void Update()
    {
        TI.clear();
        foreach (ConfigurableJoint C in CF)
        {
            collisionCheck CC = C.connectedBody.GetComponent<collisionCheck>();
            List<GameObject> col = new List<GameObject>();
            if (CC != null)
            {
                col = CC.collisions;
            }
            TI.addJoint(C.connectedBody.name, C.targetRotation, col);

        }

        TI.setWrist(RR.name, RR.transform.rotation, RR.transform.position);


    }
}

[Serializable]
public class targetInfo
{
    [SerializeField] public int ID;
    [SerializeField] public List<jointInfo> joints;
    [SerializeField] public wristInfo wrist;


    public targetInfo()
    {

        ID = 0;
        joints = new List<jointInfo>();
        wrist = new wristInfo();

    }

    public void addJoint(string name, Quaternion rot, List<GameObject> col)
    {
        
        joints.Add(new jointInfo(name, rot, col));
        
    }

    public void setWrist(string name, Quaternion rot, Vector3 pos)
    {

        wrist.name = name;
        wrist.targetRot = rot;
        wrist.targetPos = pos;
    }

    public void clear()
    {

        joints.Clear();

    }


}

[Serializable]
public class jointInfo
{
    //name of joint
    //target rotation 

    [SerializeField] public string name;
    [SerializeField] public Quaternion targetRot;
    [SerializeField] public List<String> collisions;


    public jointInfo()
    {
        name = "N/A";
        targetRot = Quaternion.identity;
        collisions = new List<String>();
    }

    public jointInfo(string namein, Quaternion rot, List<GameObject> col)
    {
        name = namein;
        targetRot = rot;
        collisions = new List<String>();
        foreach(GameObject GC in col)
        {
            collisions.Add(GC.name);

        }
    }


}

[Serializable]
public class wristInfo
{
    [SerializeField] public string name;
    [SerializeField] public Quaternion targetRot;
    [SerializeField] public Vector3 targetPos;

    public wristInfo()
    {
        name = "N/A";
        targetRot = Quaternion.identity;
        targetPos = Vector3.zero;
    }

    public wristInfo(string namein, Quaternion rot, Vector3 pos)
    {
        name = namein;
        targetRot = rot;
        targetPos = pos;
    }


}