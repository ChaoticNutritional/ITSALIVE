using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    void Start()
    {
        //SetTaskText(PutThingText);
    }

    private void Awake()
    {
        TaskActivate += OnActivation;
        potions[0] = redPotion;
        potions[1] = bluePotion;
        potions[2] = greenPotion;
    }

    //Randomly select potion from the list.
    public GameObject ChoosePotion()
    {
        int potionIndex = Random.Range(0, potions.Length);
        return potions[potionIndex];
    }

    //set object to be put
    public void SetSource(GameObject intendedSource)
    {
        toBePut = intendedSource;
    }

    public void OnActivation()
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
    


    public void OnTriggerEnter(Collider collision)
    {
        if (isActive == true)
        {
            if (collision.gameObject.name == toBePut.gameObject.name)
            {
                correctStatus = true;
                isActive = false;
            }

            else
            {
                correctStatus = false;
                isActive = false;
            }

            SetCompletion(correctStatus);
        }

        else
        {
            //Event that tells the current Task that the player did something wrong 

        }

        Destroy(collision);
    }
}
