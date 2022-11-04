using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour
{
    public bool correctStatus;
    public bool isActive = false;
    public string taskText;
    public bool completeStatus;

    public delegate void TaskCompletion();
    public static event TaskCompletion OnComplete;

    public delegate void TaskActivation();
    public static event TaskActivation TaskActivate;

    //Set active status
    public void SetActive()
    {
        TaskActivate();
        isActive = true;
    }

    //Set Correct status
    public void SetCorrectStatus(bool correct)
    {
        correctStatus = correct;
    }

    //Get correct status
    public bool GetCorrectStatus()
    {
        return correctStatus;
    }

    //set TaskText
    public void SetTaskText(string text)
    {
        taskText = text;
    }

    //Get Task Text
    public string GetTaskText()
    {
        return taskText;
    }


    //set Complete variable
    public void SetCompletion(bool complete)
    {
        completeStatus = complete;
        OnComplete();
    }

    //get complete variable
    public bool GetCompletion()
    {
        return completeStatus;
    }
}
