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
        //Activate vocalization
    }

    //Subscribed to the Completion event located in the task class
    public void OnCompletion()
    {
        Debug.Log(currentTaskIndex + "ST TASK WAS COMPLETED");

        //get the current task's correctness value and put it in the correctness array
        taskCorrect[currentTaskIndex] = currentTask.GetCorrectStatus();

        if(taskCorrect[currentTaskIndex] == false)
        {
            Debug.Log("You did the wrong task");
            //play sad animation
        }
        else
        {
            Debug.Log("You did the right task");
            //play happy animation
        }

        //increase the currentTask index
        if(currentTaskIndex < taskList.Length - 1)
        {
            //set current task =to the next task in task List
            currentTaskIndex += 1;
            currentTask = taskList[currentTaskIndex];
            currentTask.SetActive();
            TaskPrompt();
        }
        else
        {
            TallyCorrects();
        }
       

    }

    void Update()
    {
        //if any other task gets compelted while its not active
        //set current task correct to false
        //move to next task.
    }

    public void TaskPrompt()
    {
        Debug.Log("CALLING FROM TASK MGR. Task text = " + (currentTask.GetTaskText()));
        //Have the doctor display the current task's text
        DoctorText.text = currentTask.GetTaskText();
    }

    public void TallyCorrects()
    {
        Debug.Log("Counting your corrects");

        //Stop all animations
        //Play some sort of anticipation sounds
        //Trigger doctor scheming animation
        int scoreCount = 0;

        for (int i = 0; i < taskCorrect.Length; i++)
        {
            if (taskCorrect[i] == true)
            {
                scoreCount++;
            }
        }
        
        Debug.Log("Your score: " + scoreCount);

        // after tallying score, check if we have won or lost
        GetWinState(scoreCount, taskCorrect);
    }

    public bool GetWinState(int points, bool[] taskArray)
    {
        if (points < taskArray.Length)
        {
            //monster gets up and disintegrates
            //after this animation happens, Doctor is sad
            //Doctor sighs and says I guess we'll just have to try again.
            //Doctor says "We'll have to find a fresh corpse to work with. And I happen to know just where to get one." and points at player
            //Fade screen to black, return to menu

            return false;
        }

        else
        {
            //monster gets up and dances, play puttin on the ritz instrumental
            //doctor cheers, anim and sound
            //doctor stands idle
            //Doctor says "thank you for your help assistant, I couldn't have done this without you"
            //Monster turns to Doctor and begins to look aggressive.
            //Doctor fear animation
            //Doctor fall animation as Monster pursues Doctor
            //fade screen to black

            return true;
        }
    }
}
