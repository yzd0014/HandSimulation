using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class frogSpawn : MonoBehaviour
{

    public GameObject frog;
    int frameCount;
    public int maxCount;
    public float launchForce;

    public bool playing;

    // Start is called before the first frame update
    void Start()
    {

        frameCount = 0;
        
    }

    // Update is called once per frame
    void Update()
    {

        if (playing)
            frameCount++;
        else
            playing = Input.GetKeyDown(KeyCode.Space);

        if(frameCount == maxCount)
        {

            GameObject A = Instantiate(frog, transform.position, transform.rotation);
            A.GetComponent<Rigidbody>().AddForce(A.transform.forward * launchForce, ForceMode.Impulse);
            A.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(0f, 20f), Random.Range(0f, 20f), Random.Range(0f, 20f)), ForceMode.Impulse);
            frameCount = 0;

        }


    }
}
