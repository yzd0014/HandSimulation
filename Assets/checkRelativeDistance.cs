using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class checkRelativeDistance : MonoBehaviour
{

    public Transform parent;
    public Vector3 localPosition;
    public float distance;
    public Text testText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        localPosition = parent.InverseTransformPoint(transform.position);
        distance = localPosition.x;

        testText.text = distance.ToString();
    }
}
