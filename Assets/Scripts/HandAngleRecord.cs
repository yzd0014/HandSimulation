using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandAngleRecord : MonoBehaviour
{

    public List<ConfigurableJoint> ConfigJoints;
    public List<Transform> TransformTest;

    public Transform Hand;

    public List<Text> TextList;
    // Start is called before the first frame update
    void Start()
    {

        ConfigJoints = new List<ConfigurableJoint>();

        foreach(Transform child in Hand.GetComponentsInChildren<Transform>())
        {

            if (child.name.Contains("knuckle") || child.name.Contains("middle") || child.name.Contains("Palm") || child.name.Contains("Wrist"))
            {
                ConfigurableJoint[] CF = child.GetComponents<ConfigurableJoint>();
                foreach(ConfigurableJoint joint in CF)
                {
                    ConfigJoints.Add(joint);
                }

            }

        }

        TextList = new List<Text>();

        foreach(Transform child in transform.GetComponentsInChildren<Transform>())
        {
            if (child.name.Contains("Value"))
            {

                TextList.Add(child.GetComponent<Text>());

            }


        }

    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < ConfigJoints.Count; i++)
        {

            Vector3Int Angles = Vector3Int.RoundToInt(ConfigJoints[i].targetRotation.eulerAngles);
            Angles.x = Angles.x <= 180 ? Angles.x : Angles.x - 360;
            TextList[i].text = (Angles.x).ToString();
        }
        
    }
}
