using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushButtonTask : Task
{
    [SerializeField]
    private string ButtonText = "PUSH THE BUTTON PLEASE!";

    // Start is called before the first frame update
    void Start()
    {
        SetTaskText(ButtonText);
    }



    // Update is called once per frame
    void Update()
    {
        if (isActive == true)
        {
            correctStatus = true;
            isActive = false;
        }

        else
        {
            correctStatus = false;
            isActive = false;
        }
    }
}
