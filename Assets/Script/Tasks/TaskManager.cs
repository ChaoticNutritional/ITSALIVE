using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{

    //an array of booleans to show whether task was completed successfully
    [SerializeField]
    private bool[] taskCorrect = new bool[3];

    //an array of strings to select dialogue options
    public string[] taskPrompt = new string[5]; //MIGHT NOT NEED

    //array of our tasks in this playthrough
    public Task[] taskList = new Task[3];

    //array of all possible tasks 
    public Task[] allTasks = new Task[5];

    //a random number between 0 and the length - 1 of the strings/tasks array
    public int taskID; //might not need

    //a boolean of whether or not the game has been won
    public bool winState;

    //something to hold the task we're currently working on
    public Task currentTask;

    //current task index;
    public int currentTaskIndex;

    //narrator's text
    public Text DoctorText;


    void Start()
    {
        Task.OnComplete += OnCompletion;
    }

    private void Awake()
    {
        for (int i = 0; i < taskList.Length; i++)
        {
            taskID = Mathf.Abs(Random.Range(0, allTasks.Length));

            Debug.Log(taskID);

            taskList[i] = allTasks[taskID];
        }

        currentTaskIndex = 0;
        currentTask = taskList[currentTaskIndex];
        currentTask.SetActive(); //sets active variable to true;
        Invoke("OnActivation", 0.0f);
    }

    
    public void OnActivation()
    {
        TaskPrompt();
    }

    //Subscribed to the Completion event located in the task class
    public void OnCompletion()
    {
        Debug.Log(currentTaskIndex + "ST TASK WAS COMPLETED");

        //get the current task's correctness value and put it in the correctness array
        taskCorrect[currentTaskIndex] = currentTask.GetCorrectStatus();

        if(taskCorrect[currentTaskIndex] == false)
        {
            //play sad animation
        }
        else
        {
            //play happy animation
        }

        //increase the currentTask index
        currentTaskIndex += 1;

        //set current task =to the next task in task List
        currentTask = taskList[currentTaskIndex];
        currentTask.SetActive();
        TaskPrompt();
    }

    void Update()
    {
        //if any other task gets compelted while its not active
        //set current task correct to false
        //move to next task.
    }

    public void TaskPrompt()
    {
        //Have the doctor display the current task's text
        DoctorText.text = currentTask.GetTaskText();
    }

    public void TallyCorrects()
    {
        int scoreCount = 0;

        for (int i = 0; i < taskCorrect.Length; i++)
        {
            if (taskCorrect[i] == true)
            {
                scoreCount++;
            }
        }

        // after tallying score, check if we have won or lost
        GetWinState(scoreCount, taskCorrect);
    }

    public bool GetWinState(int points, bool[] taskArray)
    {
        if (points < taskArray.Length)
        {
            return false;
        }

        else
        {
            return true;
        }
    }
}
