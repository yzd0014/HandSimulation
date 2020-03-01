using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class forceConstants : MonoBehaviour
{


    public List<rigidRotate> RR;
    //public ThumbRigidRotate TRR;
    // Start is called before the first frame update


    public float MaxForceChange;
    public float ForceMagic;
    public float MaxTorqueChange;
    public float TorqueMagic;

    public float ForceDamp;
    public float TorqueDamp;

    void Start()
    {
        
        foreach(rigidRotate rr in RR)
        {
            //rr.MaxForceChange = MaxForceChange;
            rr.ForceStiffness = ForceMagic;
            //rr.MaxTorqueChange = MaxTorqueChange;
            rr.TorqueStiffness = TorqueMagic;
            rr.ForceDamp = ForceDamp;
            rr.TorqueDamp = TorqueDamp;


        }

        /*
        TRR.MaxForceChange = MaxForceChange;
        TRR.ForceMagic = ForceMagic;
        TRR.MaxTorqueChange = MaxTorqueChange;
        TRR.TorqueMagic = TorqueMagic;
        TRR.ForceDamp = ForceDamp;
        TRR.TorqueDamp = TorqueDamp;
*/

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
