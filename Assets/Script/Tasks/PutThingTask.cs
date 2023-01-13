﻿using System.Collections;
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
    public AudioSource potAudio;

    [SerializeField]
    private GameObject[] potions = new GameObject[3];
    private bool[] beenChosen = new bool[3];


    //fill the potions array at start

    private void Awake()
    {
        TaskActivate += OnPotionActivation;
        potions[0] = redPotion;
        potions[1] = bluePotion;
        potions[2] = greenPotion;

        for(int i = 0; i < 3; i++)
        {
            beenChosen[0] = false;
        }
    }

    //Randomly select potion from the list.
    public GameObject ChoosePotion()
    {
        int potionIndex = Random.Range(0, potions.Length);
        //if potions[potionIndex] is in some array of colors already chosen, choose a different potion color
        

        //OR
        while(beenChosen[potionIndex])
        {
            potionIndex = Random.Range(0, potions.Length);
        }

        Debug.Log("I choose: " + potions[potionIndex]);
        thingToPut = potions[potionIndex].name;
        beenChosen[potionIndex] = true;

        return potions[potionIndex];
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

        SetSource(currentPotion);

        //Manager will use the GetTaskText() function to gain access to this string
    }
    
    //This is what will complete the task
    public void OnTriggerEnter(Collider collision)
    {
        
        if (collision.gameObject.CompareTag("Potion") && !collision.gameObject.CompareTag("Controller"))
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
                    Debug.Log(collision.name + "hit the pot");
                }
                //else, the correct status remains false on the current object
                SetCorrectStatus(correctStatus);
                isActive = false;
            }

            else
            {
                correctStatus = false;
                isActive = false;
            }
            //destroy the object you threw in the pot

            potAudio.Play();
            potion?.GetComponent<XRGrabInteractable>()?.colliders.Clear();
            Destroy(potion);


            //Activate smoke rising from cauldron. PENDING

            //Play sizzling sound DONE

            //Complete a task regardless of its correct status DONE
            TaskCompleted();
        }
    }
}
