using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;
using System;
using Leap.Unity.Attributes;
using Leap.Unity;
using Leap;
using System.IO;

public class LeapSerializer : MonoBehaviour
{

    [Tooltip("The LeapProvider to use to drive hand representations in the defined "
   + "model pool groups.")]
    [SerializeField]
    [OnEditorChange("leapProvider")]
    public LeapProvider leapProvider;

    public bool isRecording;
    public frameList recordedFrames;

    public FocusFinder FF;
    public HandFinder LHH;
    public HandFinder RHH;

    private void Start()
    {
        recordedFrames = new frameList();
    }

    private void OnEnable()
    {
        leapProvider.OnUpdateFrame += OnUpdateFrame;
    }

    private void OnDisable()
    {
        leapProvider.OnUpdateFrame -= OnUpdateFrame;
    }

    public Renderer recLight;



    /** Updates the graphics HandRepresentations. */
    protected virtual void OnUpdateFrame(Frame frame)
    {
        
        //after focus data, we also want our targert rotations from our config joints 
        //and also the writst stuff

        if (isRecording)
        {
            //Debug.Log("here");
            recordedFrames.add(frame, FF.focusData, RHH.TI, LHH.TI);
            
        }

        

    }

    public void StartRecording()
    {
        isRecording = true;
        recLight.material.SetColor("_EmissionColor", Color.red);
        recordedFrames.startFrame = Time.frameCount;
    
    }

    public void EndRecording()
    {
        isRecording = false;
        recLight.material.SetColor("_EmissionColor", Color.black);
        recordedFrames.endFrame = Time.frameCount;

        string json = JsonUtility.ToJson(recordedFrames);
        //Debug.Log(json);
        File.WriteAllText(Application.dataPath + "/" + System.DateTime.UtcNow.ToFileTime().ToString() + ".txt", json);
        recordedFrames.clear();


    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)){

            if (isRecording)
            {
                EndRecording();
            }
            else
            {
                StartRecording();
            }

        }
    }




}

[Serializable]
public class frameList
{

    [SerializeField] public int startFrame;
    [SerializeField] public int endFrame;
    [SerializeField] public List<Frame> frames;
    [SerializeField] public List<FocusData> focuses;
    [SerializeField] public List<targetInfo> LeftHand;
    [SerializeField] public List<targetInfo> RightHand;


    public frameList()
    {
        startFrame = 0;
        endFrame = 0;
        frames = new List<Frame>();
        focuses = new List<FocusData>();
        LeftHand = new List<targetInfo>();
        RightHand = new List<targetInfo>();
    }

    public void add(Frame frame, FocusData focus, targetInfo rightHand, targetInfo leftHand)
    {
        frames.Add(frame);
        focuses.Add(focus);
        RightHand.Add(rightHand);
        LeftHand.Add(leftHand);
    }
    public void clear()
    {
        startFrame = 0;
        endFrame = 0;
        frames.Clear();
        focuses.Clear();
        RightHand.Clear();
        LeftHand.Clear();
    }

}




