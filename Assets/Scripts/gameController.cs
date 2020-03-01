using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameController : MonoBehaviour
{


    public List<GameObject> resetables;
    public List<Vector3> resetPositions;
    public List<Quaternion> resetRotations;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject O in resetables)
        {
            resetPositions.Add(O.transform.position);
            resetRotations.Add(O.transform.rotation);
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            reset();


        }
    }

    private void reset()
    {
        int idx = 0;
        foreach(GameObject O in resetables)
        {
            O.transform.position = resetPositions[idx];
            O.transform.rotation = resetRotations[idx];
            Rigidbody RB;
            bool isRB = O.TryGetComponent<Rigidbody>(out RB);
            if (isRB)
            {
                RB.velocity = Vector3.zero;
                RB.angularVelocity = Vector3.zero;

            }
            idx++;

        }


    }
}
