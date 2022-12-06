using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderTask : Task
{
    [SerializeField]
    private string SliderText = "PUSH THE BUTTON PLEASE!";

    // Start is called before the first frame update
    void Awake()
    {
        SetTaskText(SliderText);
    }

    public void OnSliderTaskActivation()
    {
        if (isActive)
        {
            correctStatus = true;
            Debug.Log("You pulled the slider when we asked you to:)");
        }

        else
        {
            Debug.Log("You pulled the slider even though we didn't ask you to :(");
        }
        TaskCompleted();
    }


}
