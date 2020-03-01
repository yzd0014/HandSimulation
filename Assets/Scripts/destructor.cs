using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destructor : MonoBehaviour
{

    int timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;   
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer++;
        if(timer > 1)
        {
            Destroy(transform.gameObject);

        }

    }
}
