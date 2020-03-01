using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
    bool on;
    bool Grabbing;
    bool releasing;

    List<List<Transform>> fingers;

    public List<Transform> pointer;
    public List<Transform> middle;
    public List<Transform> ring;
    public List<Transform> pinky;

    public float speed;

    // Start is called before the first frame update
    void Start()
    {

        fingers = new List<List<Transform>>();
        fingers.Add(pointer);
        fingers.Add(middle);
        fingers.Add(ring);
        fingers.Add(pinky);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!on)
            {
                on = true;
                StartCoroutine("curl");
            }
        }  
    }

    IEnumerator curl()
    {
        float t = 0;

        while(t < 1)
        {
            curlMix(t);
            yield return null;
            t += Time.deltaTime * speed;
        }
        curlMix(1);
    }

    void curlMix(float curl)
    {

        foreach(List<Transform> finger in fingers)
        {

            foreach(Transform joint in finger)
            {

                Quaternion newRot = Quaternion.Slerp(Quaternion.Euler(0,0,0), Quaternion.Euler(90, 0, 0), curl);
                joint.localRotation = newRot;

            }

        }


    }
}
