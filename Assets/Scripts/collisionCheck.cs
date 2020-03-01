using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionCheck : MonoBehaviour
{

    public bool touch;
    public List<GameObject> collisions = new List<GameObject>();


    void OnCollisionEnter(Collision col)
    {

        // Add the GameObject collided with to the list.
        collisions.Add(col.gameObject);
        touch = true;
        // Print the entire list to the console.

    }

    private void OnCollisionExit(Collision collision)
    {
        collisions.Remove(collision.gameObject);
        touch = false;  
        
    }
}
