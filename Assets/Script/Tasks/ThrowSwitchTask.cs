using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowSwitchTask : Task
{
    [SerializeField]
    private string LeverText = "Pull the lever please!";

    public Lever yourLever = null;
    public Transform leverCachedPos;

    public bool stoppedMoving = false;

    public Transform myStartPos;
    public Transform myEndPos;

    // Start is called before the first frame update

    private void Start()
    {
        myStartPos = yourLever.startOrientation;
        myEndPos = yourLever.endOrientation;
        leverCachedPos = yourLever.transform;
    }

    void Awake()
    {
        SetTaskText(LeverText);
    }

    public void StoppedMoving()
    {
        if(leverCachedPos.rotation == myEndPos.rotation || yourLever.transform.rotation == myStartPos.rotation)
        {
            OnActivation();
        }
    }

    public void OnActivation()
    {
        //if lever is at upright or down position
            if (isActive)
            {
                correctStatus = true;
                Debug.Log("You pulled the lever when we told you to :)");
            }

            else
            {
                Debug.Log("You pulled the lever when we didn't tell you to :(");
            }
        TaskCompleted();
    }


}
