using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


public class DissolveController : MonoBehaviour
{
    //bool alive = true;
    public Animator animator;
    public VisualEffect smokeburn;
    public bool BurstIntoFlames = false;
    public GameObject yourGuy;

    public Material[] skinnedMaterials;
    public Material[] originalMaterials;
    public float dissolveRate = 0.00125f;
    public float refreshRate = 0.025f;
    //private Material[] tempMaterials;
    float counter = 0f;

    void Start()
    {

    }

    void Update()
    {
        if(BurstIntoFlames)
        {
            StartCoroutine(DissolveCondition());
        }
    }

    public void BurnUp()
    {
        BurstIntoFlames = true;
        smokeburn.Play();
    }

    private void OnApplicationQuit()
    {
        for(int i = 0; i < skinnedMaterials.Length; i++)
        {
            skinnedMaterials[i].CopyPropertiesFromMaterial(originalMaterials[i]);
        }
    }

    IEnumerator DissolveCondition()
    {
        if(skinnedMaterials.Length > 0)
        {
            while (skinnedMaterials[0].GetFloat("_dissolveAmt") < 1)
            {
                //decrease;
                counter += dissolveRate;
                for(int i = 0; i < skinnedMaterials.Length; i++)
                {
                    skinnedMaterials[i].SetFloat("_dissolveAmt", counter);
                }
                yield return new WaitForEndOfFrame();
            }
        }
        DisableGuy();
        BurstIntoFlames = false;
    }

    public void DisableGuy()
    {
        StopCoroutine(DissolveCondition());
        /*for (int j = 0; j < skinnedMaterials.Length; j++)
        {
            skinnedMaterials[j].SetFloat("_dissolveAmt", 0);
        }*/
    }
}
