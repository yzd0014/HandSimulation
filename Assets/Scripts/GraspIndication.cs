using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraspIndication : MonoBehaviour
{


    //need a list of every object we get local rotations from
    //get the X value
    //average it

    public GameObject fudger;
    public Transform fudgerspawn;

    public List<Transform> HandRotations;
    public Transform Hand;
    public collisionCheck[] CCs;
    public List<Transform> pieces;
    public Text avgText;
    float avg;
    float pAvg;
    float trend;

    public int maxReleaseTimer;
    public bool released;

    float releasedCol = 1;

    List<Vector3> currentFlashColor;
    List<Color> currentBaseColor;
    public List<Renderer> mats;

    public float trendThreshold;
    public float graspThreshold;

    private void Start()
    {
        mats = new List<Renderer>();

        foreach(Transform child in Hand.GetComponentsInChildren<Transform>())
        {
            if (child.tag.Equals("handPiece"))
                mats.Add(child.GetComponent<Renderer>());
            if (child.gameObject.layer == 8)
                pieces.Add(child);
        }

        CCs = Hand.GetComponentsInChildren<collisionCheck>();
        currentFlashColor = new List<Vector3>();
        currentBaseColor = new List<Color>();

        for(int i = 0; i < mats.Count; i++)
        {

            currentBaseColor.Add(Color.gray);
            currentFlashColor.Add(Vector3.zero);

        }
    }


    private Color colorStep(int idx, bool contact)
    {
        //get our flash color
        currentFlashColor[idx] = Vector3.Lerp(currentFlashColor[idx], Vector3.zero, 0.06f);
        currentBaseColor[idx] = contact ? Color.blue : Color.gray;

        Color returnColor = currentBaseColor[idx];
        returnColor.r += currentFlashColor[idx].x;
        returnColor.g += currentFlashColor[idx].y;
        returnColor.b = contact ? returnColor.b : returnColor.b + currentFlashColor[idx].z;
        returnColor.a = releasedCol;

        return returnColor;
    }

    private void FixedUpdate()
    {
        avg = 0;
        foreach(Transform T in HandRotations)
        {
            float angleIn = T.localRotation.eulerAngles.x;
            angleIn = angleIn > 180 ? angleIn - 360 : angleIn;
            avg += angleIn;

        }
        avg = avg / HandRotations.Count;
        //avgText.text = ((int)avg).ToString();


        if (avg < pAvg)
        {
            trend += pAvg - avg;
        }
        else
        {

            trend = 0;
        }

        int idx = 0;

        bool A = false;
        bool B = false;

        foreach (Renderer R in mats)
        {
            if (trend > trendThreshold && pAvg > graspThreshold)
            {
                currentFlashColor[idx] = new Vector3(1, -1, -1);
                A = true;
            }

            if (CCs[idx].touch)
                B = true;
            R.material.SetColor("_Color", colorStep(idx, CCs[idx].touch));
            idx++;
        }

        if((A && B) && !released)
        {
            released = true;

            foreach(Transform T in pieces)
            {

                T.gameObject.layer = 10;
                

            }
            Instantiate(fudger, fudgerspawn.position, Quaternion.identity);
            StartCoroutine(releaseTimer());


        }


        pAvg = avg;

    }

    public IEnumerator releaseTimer()
    {
        
        int A = 0;
        while(A < maxReleaseTimer)
        {

            A++;
            releasedCol = (Mathf.Sign(Mathf.Sin(Time.deltaTime * A * 150))) + 1;
            yield return null;
        }

        releasedCol = 1;
        foreach (Transform T in pieces)
        {

            T.gameObject.layer = 8;


        }
        released = false;

    }

}
