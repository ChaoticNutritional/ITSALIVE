using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskManager : MonoBehaviour
{

    //an array of booleans to show whether task was completed successfully
    [SerializeField]
    private bool[] taskComplete = new bool[3];

    //an array of strings to select dialogue options
    public string[] taskPrompt = new string[5];

    //array of our tasks in this playthrough
    public Task[] taskList = new Task[3];

    //array of all possible tasks 
    public Task[] allTasks = new Task[5];

    //a random number between 0 and the length - 1 of the strings/tasks array
    public int taskID;

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
        currentTask.SetActive();
        Invoke("OnActivation", 0.0f);

        Task.TaskActivate += CurrentTaskListener;
    }

    //
    public void OnActivation()
    {
        TaskPrompt();
    }

    //Subscribed to the Completion event located in the task class
    public void OnCompletion()
    {
        currentTaskIndex += 1;
        currentTask = taskList[currentTaskIndex];
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CurrentTaskListener()
    {

    }

    
    public void TaskPrompt()
    {
        DoctorText.text = currentTask.GetTaskText();
        Debug.Log(currentTask.GetTaskText());
    }

    public void TallyCorrects()
    {
        int scoreCount = 0;

        for (int i = 0; i < taskComplete.Length; i++)
        {
            if (taskComplete[i] == true)
            {
                scoreCount++;
            }
        }

        // after tallying score, check if we have won or lost
        GetWinState(scoreCount, taskComplete);
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
