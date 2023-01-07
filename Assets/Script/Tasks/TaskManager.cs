using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    public Animation[] happyAnims;
    public Animation[] sadAnims;

    public string[] onboardingText;
    public AudioClip[] onboardingAudio;

    public Animator corpseStand;
    public GameObject newFrank;
    public GameObject frankieHolder;

    public Animator DoctorAnim;
    public AudioSource DoctorAudio;

    public GameObject dancingFrank;
    

    public struct Onboarding
    {
        string aText;
        AudioClip aSound;
    }

    public Onboarding[] OnboardingPool;

    public DissolveController dissolveController;


    void Start()
    {
        
    }

    private void Awake()
    {
        StartCoroutine(WaitAFew());
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

    public void CurrentTaskListener()
    {

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
        StartCoroutine(RiseAndWalk());
        

        //LOSE GAME
        if (points < taskArray.Length)
        {
            //NEEDS:

            //Monster gets up from table; || accomplished by animation


            //Unparent monster from table || accomplished by animation event

            //Reference to table animator  || DONE
            //EndRaise trigger for table animator true; || DONE

            newFrank.GetComponent<Animator>().SetBool("DidWeWin", false);
            StartCoroutine(AngryResult());

            
            

            //monster gets up and disintegrates
            //after this animation happens, Doctor is sad
            //Doctor sighs and says I guess we'll just have to try again.
            //Doctor says "We'll have to find a fresh corpse to work with. And I happen to know just where to get one." and points at player
            //Fade screen to black, return to menu


            return false;
        }

        //WIN GAME
        else
        {
            newFrank.GetComponent<Animator>().SetBool("DidWeWin", true);
            StartCoroutine(WatchHimDance());


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

    IEnumerator WaitAFew()
    {
        yield return new WaitForSeconds(3);
        StartCoroutine(OnBoarding());

        //play sound
        //activate animation
        //change dialogue
    }

    //Present onboarding text and sounds to player
    IEnumerator OnBoarding()
    {
        int counter = 0;

        while (counter < onboardingText.Length)
        {

            DoctorText.text = onboardingText[counter];
            DoctorAudio.PlayOneShot(onboardingAudio[Random.Range(0, onboardingAudio.Length)]);
            new WaitForSeconds(7);
            counter++;
            yield return new WaitForSeconds(7);
        }

        DoctorText.text = "Now.... Let's begin...";
        new WaitForSeconds(5);
        StartCoroutine(TaskSpinup());
    }

    IEnumerator TaskSpinup()
    {
        yield return new WaitForSeconds(5);

        Task.OnComplete += OnCompletion;



        for (int i = 0; i < taskList.Length; i++)
        {
            taskID = Mathf.Abs(Random.Range(0, allTasks.Length));

            Debug.Log(taskID);

            taskList[i] = allTasks[taskID];
        }

        currentTaskIndex = 0;
        currentTask = taskList[currentTaskIndex];
        while (currentTask == null)
        {
            Debug.Log("waiting for current task to load");
        }
        currentTask?.SetActive();  //sets active variable to true;
        Invoke("OnActivation", 0.0f);
    }

    IEnumerator RiseAndWalk()
    {
        corpseStand.SetTrigger("EndRaise");
        DoctorAnim.SetTrigger("LookOn");
        yield return new WaitForSeconds(3);

        newFrank.GetComponent<Animator>().SetTrigger("getUp");
        //newFrank.transform.parent = frankieHolder.transform;
        newFrank.transform.SetPositionAndRotation(new Vector3(3.633754f, -0.130233f, 0.7654648f), Quaternion.Euler(0f, 0f, 0f));
        newFrank.transform.parent = null;


    }

    IEnumerator WatchHimDance()
    {
        //doctor animator trigger look
        yield return new WaitForSeconds(3);

        //doctor animator trigger cheer
        DoctorAnim.SetTrigger("Dance");
        


        //doctor animator trigger silly dance

        //doctor animator trigger talk

        DoctorText.text = "You did it! Great job assistant! Goodbye!";
        DoctorAudio.PlayOneShot(onboardingAudio[Random.Range(0, onboardingAudio.Length)]);
        yield return new WaitForSeconds(12);

        //return to menu
        ReturnToMenu();

    }

    private void ReturnToMenu()
    {
        newFrank.transform.parent = frankieHolder.transform;
        SceneLoader.Instance.LoadNewScene("MenuScene");
        StopCoroutine(WatchHimDance());

        
    }

    IEnumerator AngryResult()
    {
        newFrank.transform.parent = frankieHolder.transform;
        yield return new WaitForSeconds(2);

        DoctorText.text = "NO!!!";
        yield return new WaitForSeconds(2);

        DoctorText.text = "We were so close.";
        DoctorAudio.PlayOneShot(onboardingAudio[Random.Range(0, onboardingAudio.Length)]);

        yield return new WaitForSeconds(3);

        DoctorText.text = "Now I'll need to start from scratch with another body....";
        DoctorAudio.PlayOneShot(onboardingAudio[Random.Range(0, onboardingAudio.Length)]);

        yield return new WaitForSeconds(3);

        DoctorAnim.SetTrigger("AngryApproach");
        yield return new WaitForSeconds(1);
        DoctorText.text = "And I think I know just where to find one..... heheheheh";
        
        DoctorAudio.PlayOneShot(onboardingAudio[Random.Range(0, onboardingAudio.Length)]);


        yield return new WaitForSeconds(2);

        ReturnToMenu();
    }
}
