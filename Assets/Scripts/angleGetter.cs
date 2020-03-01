using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class angleGetter : MonoBehaviour
{

    public Text value;
    public Transform Hands;

    float pAvg;
    float trend;
    // Start is called before the first frame update

    // Update is called once per frame
    void FixedUpdate()
    {
        HingeJoint[] A = Hands.GetComponentsInChildren<HingeJoint>();
        float avg = 0;
        foreach(HingeJoint H in A)
        {

          avg += H.spring.targetPosition;

        }
        avg = avg / A.Length;
        value.text = ((int)avg).ToString();


        if(avg > pAvg)
        {
            trend += avg - pAvg;
        }
        else
        {

            trend = 0;
        }

        Renderer[] R = Hands.GetComponentsInChildren<Renderer>();
        foreach (Renderer M in R)
        {
            if (trend > 10 && pAvg < -10)
            {
                M.material.SetColor("_Color", new Color(1, 0, 0, 1));

            }
            M.material.SetColor("_Color", Color.Lerp(M.material.GetColor("_Color"), Color.grey, 0.03f));
        }
        pAvg = avg;
        
    }
}
