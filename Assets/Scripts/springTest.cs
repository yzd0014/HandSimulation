using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class springTest : MonoBehaviour
{

    public Rigidbody RB;
    public Rigidbody Outboard;
    public StrutInfo OutInfo;
    public Rigidbody Inboard;
    public StrutInfo InInfo;

    public Vector3 InAnchor;

    public float strength;
    public float damp;
    // Start is called before the first frame update
    void Start()
    {

        if (Outboard != null)
        {
            OutInfo = new StrutInfo((RB.position - Outboard.position).magnitude, 100, 10);
        }

        if (Inboard != null)
        {
            InInfo = new StrutInfo((RB.position - (Inboard.transform.TransformPoint(InAnchor))).magnitude, 100, 10);
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateSprings();
    }

    void UpdateSprings()
    {

        if (Outboard != null)
        {
            Vector3 vec = RB.position - Outboard.position;
            float length = vec.magnitude;
            Vector3 force = strength * (length - OutInfo.restingLength) * vec.normalized;
            Vector3 damping = damp * (Vector3.Dot((Outboard.velocity - RB.velocity), vec.normalized)) * vec.normalized;
            RB.AddForceAtPosition(-force + damping, Outboard.position);
            Outboard.AddForceAtPosition(force - damping, Outboard.position);
        }

        if (Inboard != null)
        {
            Vector3 vec = RB.position - (Inboard.transform.TransformPoint(InAnchor));
            Debug.DrawLine(Inboard.position, Inboard.transform.TransformPoint(InAnchor));
            float length = vec.magnitude;

            Vector3 force = strength * (length - InInfo.restingLength) * vec.normalized;
            Vector3 damping = damp * (Vector3.Dot((Inboard.velocity - RB.velocity), vec.normalized)) * vec.normalized;
            RB.AddForceAtPosition(-force + damping, RB.position);
            Inboard.AddForceAtPosition(force + -damping, RB.position);
        }

    }
}
public class StrutInfo
{

    public float restingLength;
    public float strength;
    public float damp;

    public StrutInfo(float RL, float S, float d)
    {

        restingLength = RL;
        strength = S;
        damp = d;

    }






}

