using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PutThingTask : Task
{
    private GameObject toBePut = null;

    [SerializeField]
    private string PutThingText;
    private string thingToPut;

    public GameObject redPotion;
    public GameObject bluePotion;
    public GameObject greenPotion;

    [SerializeField]
    private GameObject[] potions = new GameObject[3];


    //fill the potions array at start

    private void Awake()
    {
        TaskActivate += OnPotionActivation;
        potions[0] = redPotion;
        potions[1] = bluePotion;
        potions[2] = greenPotion;
    }

    //Randomly select potion from the list.
    public GameObject ChoosePotion()
    {
        int potionIndex = Random.Range(0, potions.Length);
        //if potions[potionIndex] is in some array of colors already chosen, choose a different potion color
        

        //OR

        //if potions[potionIndex] isn't found in the scene, choose another one
        while(!CheckIfInScene(potions[potionIndex]))
        {
            potionIndex = Random.Range(0, potions.Length);
        }

        Debug.Log("I choose: " + potions[potionIndex]);
        thingToPut = potions[potionIndex].name;


        return potions[potionIndex];
    }

    private bool CheckIfInScene(GameObject targetPotion)
    {
        if (GameObject.Find(targetPotion.name) != null)
        {
            //it exists
            return true;
        }
        else
        {
            return false;
        }
    }

    //set object to be put
    public void SetSource(GameObject intendedSource)
    {
        toBePut = intendedSource;
    }

    public void OnPotionActivation()
    {
        //Sets source to be a randomly chosen potion color
        GameObject currentPotion = ChoosePotion();

        //sets object to put to the randomly selected potion
        SetSource(currentPotion);

        //name of current potion
        thingToPut = currentPotion.name;

        //Set text to put current potion in cauldron
        PutThingText = "Put the " + thingToPut + " in the cauldron";

        //Call function to set taskText to your taskText created in this scope
        SetTaskText(PutThingText);

        //Manager will use the GetTaskText() function to gain access to this string
    }
    
    //This is what will complete the task
    public void OnTriggerEnter(Collider collision)
    {
        if (!collision.gameObject.CompareTag("Controller"))
        {
            GameObject potion = collision.gameObject;
            if (isActive == true)
            {
                if (collision.gameObject.name == toBePut.gameObject.name)
                {
                    //is current task and you throw the right object in the pot
                    correctStatus = true;
                    Debug.Log("You put the correct object in the pot!");
                    isActive = false;
                }
                else
                {
                    Debug.Log("You put the wrong object in the pot");
                }
                //else, the correct status remains false on the current object
                SetCorrectStatus(correctStatus);
                isActive = false;
            }
            //destroy the object you threw in the pot
            potion.GetComponent<XRGrabInteractable>().colliders.Clear();
            Destroy(potion);
            Debug.Log("Destroying: " + collision.gameObject.name.ToUpper());

            //Complete a task regardless of its correct status
            TaskCompleted();
            
            Debug.Log("TASK COMPLETE");
        }
    }
}
