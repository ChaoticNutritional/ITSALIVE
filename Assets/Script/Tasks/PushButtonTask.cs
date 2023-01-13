using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushButtonTask : Task
{
    private string ButtonText = "PUSH THE BUTTON PLEASE!";

    // Start is called before the first frame update

    public void Start()
    {
       this.taskText = ButtonText;
    }

    public void BeenChosen()
    {
        Debug.Log("Button has been chosen and button text should be displayed");


    }

    public void OnButtonTaskActivation()
    {
        if(isActive)
        {
            correctStatus = true;
            Debug.Log("You pushed the button just like we said :)");
        }

        else
        {
            Debug.Log("You pressed the button when we didn't tell you to :(");
        }
        TaskCompleted();
    }


}
