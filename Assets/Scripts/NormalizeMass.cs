using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalizeMass : MonoBehaviour
{

    // need a list of out hand pieces, in some order

    // the order:
    // wrist -> rest of hand
    // palm -> fingers
    // fingers-> finger bits
    // etc
    // so we need an array that helds the values of each subsequent total mass

    

    //wrist, palm, kt, kp, km, kr, kp, mt, mp, mm, mr, mp - total of 12

    public List<Transform> pieceList;
    List<Rigidbody> RBList;
    float[] totalMassList;
    int[] indexList = { 16, 15, 2, 1, 0, 2, 1, 0, 2, 1, 0, 2, 1, 0, 2, 1, 0 };
    //wrist, palm, thumbA, B, C, pointerA, B, C, middleA, B, C, ringA, B, C, pinkyA, B, C
    //16, 15, 2, 1, 0, 2, 1, 0, 2, 1, 0, 2, 1, 0, 2, 1, 0




    void Start()
    {

        totalMassList = new float[17];
        RBList = new List<Rigidbody>();


        //get all the rigid bodies in a separate list, will make this easier
        foreach(Transform T in pieceList)
        {
            //for each piece
            Rigidbody RB = T.GetComponent<Rigidbody>();
            RBList.Add(RB);   
        }

        for(int i = 0; i < 17; i++)
        {
            float grossMass = 0;

            for(int j = 1; j <= indexList[i]; j++)
            {

                grossMass += RBList[i + j].mass;


            }
            totalMassList[i] = grossMass;

        }

        int index = 0;
        foreach(Transform T in pieceList)
        {
            CharacterJoint j;
            if (TryGetComponent<CharacterJoint>(out j))
            {

                j.massScale = totalMassList[index] / RBList[index].mass;
                //might need to change this as well, in the long run - we'll see
                j.connectedMassScale = totalMassList[index] / j.connectedBody.mass;
            }
            index++;
        }

        



        


        //Make sure that both of the connected bodies will be moved by the solver with equal speed


        //gotta rework this into a different script, I guess- need to have each body normalize ALL connected masses in the inboard

        //the character joints usually normalize outwards, so we'll normalize by outboard instead
        //we'll see how it works just by changing mass scale- will scale conencted mass to include all the other ones later, to see what happens



       


    }
}